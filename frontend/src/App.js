import React, { useState } from "react";
import axios from "axios";
import "./App.css";

function App() {
  const [message, setMessage] = useState("");
  const [messages, setMessages] = useState([]);
  const [loading, setLoading] = useState(false);

  // 🌐 API bağlantısı (Render)
  const API_URL = `${process.env.REACT_APP_API_URL}/api/messages`;

  const sendMessage = async () => {
    if (!message.trim()) return;
    setLoading(true);

    const userMsg = { text: message, sender: "user" };
    setMessages((prev) => [...prev, userMsg]);
    setMessage("");

    try {
      // 🔁 API'ye mesaj gönder
      const response = await axios.post(API_URL, { text: userMsg.text });
      const rawSentiment = response.data.sentiment || "Bilinmiyor";

      // 🔍 Sadece ilk kelimeyi al (örnek: "neutral (0.72...)" → "neutral")
      const sentiment = rawSentiment.split(" ")[0].toLowerCase();

      // 🎨 Türkçe çeviri ve renk
      let translated = "Bilinmiyor 🤔";
      let bgColor = "#e9ecef";

      if (sentiment.includes("positive")) {
        translated = "Pozitif 😊";
        bgColor = "#b7e3b0";
      } else if (sentiment.includes("negative")) {
        translated = "Negatif 😞";
        bgColor = "#f5b7b1";
      } else if (sentiment.includes("neutral")) {
        translated = "Nötr 😐";
        bgColor = "#d6d8d9";
      }

      const botMsg = { text: translated, sender: "bot", bgColor };
      setMessages((prev) => [...prev, botMsg]);
    } catch (error) {
      console.error("❌ API Hatası:", error);
      setMessages((prev) => [
        ...prev,
        { text: "⚠️ Sunucuya ulaşılamıyor.", sender: "bot", bgColor: "#f8d7da" },
      ]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container">
      <h2 className="title">🤖 AI Duygu Analizi Chat</h2>

      <div className="chat-box">
        {messages.map((msg, i) => (
          <div
            key={i}
            className={`message ${msg.sender}`}
            style={{ backgroundColor: msg.bgColor || undefined }}
          >
            {msg.text}
          </div>
        ))}
      </div>

      <div className="input-row">
        <input
          type="text"
          placeholder="Bir mesaj yaz..."
          value={message}
          onChange={(e) => setMessage(e.target.value)}
          className="input"
        />
        <button
          onClick={sendMessage}
          disabled={loading}
          className={`send-btn ${loading ? "disabled" : ""}`}
        >
          {loading ? "..." : "Gönder"}
        </button>
      </div>
    </div>
  );
}

export default App;
