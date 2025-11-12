import React, { useState } from "react";
import axios from "axios";

export default function Chat() {
  const [messages, setMessages] = useState([]);
  const [text, setText] = useState("");
  const [loading, setLoading] = useState(false);

  // .env'den backend adresini alıyoruz
  const API_URL = process.env.REACT_APP_API_BASE;

  const translateSentiment = (english) => {
    if (!english) return "belirsiz";
    english = english.toLowerCase();
    if (english.includes("positive")) return "Pozitif 😊";
    if (english.includes("negative")) return "Negatif 😞";
    if (english.includes("neutral")) return "Nötr 😐";
    return "Belirsiz";
  };

  const getSentimentColor = (english) => {
    if (!english) return "#f0f0f0";
    english = english.toLowerCase();
    if (english.includes("positive")) return "#c8f7c5"; // yeşil
    if (english.includes("negative")) return "#f7c5c5"; // kırmızı
    if (english.includes("neutral")) return "#f0f0f0"; // gri
    return "#f0f0f0";
  };

  const sendMessage = async () => {
    if (!text.trim()) return;
    setLoading(true);

    setMessages((prev) => [...prev, { sender: "user", text }]);

    try {
      // 🌐 Render'daki backend adresini otomatik kullanır
      const response = await axios.post(`${API_URL}/messages`, {
        Text: text,
      });

      const sentiment = response.data.sentiment || "unknown";
      const label = sentiment.split(" ")[0];
      const score = sentiment.match(/\(([^)]+)\)/)?.[1] || "";

      const translated = translateSentiment(label);
      const color = getSentimentColor(label);
      const formattedScore = score ? `Skor: ${parseFloat(score).toFixed(2)}` : "";

      const botText = `${translated} ${formattedScore}`;

      setMessages((prev) => [...prev, { sender: "bot", text: botText, color }]);
    } catch (error) {
      console.error("Mesaj gönderilirken hata:", error);
      setMessages((prev) => [
        ...prev,
        { sender: "bot", text: "⚠️ Sunucuya ulaşılamıyor." },
      ]);
    }

    setText("");
    setLoading(false);
  };

  const handleKeyPress = (e) => {
    if (e.key === "Enter") sendMessage();
  };

  return (
    <div style={styles.container}>
      <h2>💬 AI Duygu Analizi Chat</h2>

      <div style={styles.chatBox}>
        {messages.map((msg, index) => (
          <div
            key={index}
            style={{
              ...styles.message,
              alignSelf: msg.sender === "user" ? "flex-end" : "flex-start",
              backgroundColor:
                msg.sender === "user"
                  ? "#d1e7ff"
                  : msg.color || getSentimentColor(msg.text),
            }}
          >
            {msg.text}
          </div>
        ))}
      </div>

      <div style={styles.inputArea}>
        <input
          type="text"
          value={text}
          onChange={(e) => setText(e.target.value)}
          onKeyDown={handleKeyPress}
          placeholder="Bir mesaj yaz..."
          style={styles.input}
          disabled={loading}
        />
        <button onClick={sendMessage} style={styles.button} disabled={loading}>
          {loading ? "..." : "Gönder"}
        </button>
      </div>
    </div>
  );
}

const styles = {
  container: {
    fontFamily: "sans-serif",
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    height: "100vh",
    padding: "20px",
    backgroundColor: "#fafafa",
  },
  chatBox: {
    width: "100%",
    maxWidth: "600px",
    height: "70vh",
    display: "flex",
    flexDirection: "column",
    padding: "10px",
    border: "2px solid #ccc",
    borderRadius: "10px",
    overflowY: "auto",
    backgroundColor: "#fff",
  },
  message: {
    margin: "6px",
    padding: "10px",
    borderRadius: "8px",
    maxWidth: "80%",
    wordBreak: "break-word",
  },
  inputArea: {
    marginTop: "20px",
    display: "flex",
    width: "100%",
    maxWidth: "600px",
  },
  input: {
    flex: 1,
    padding: "10px",
    borderRadius: "8px",
    border: "1px solid #ccc",
    fontSize: "16px",
  },
  button: {
    marginLeft: "10px",
    padding: "10px 16px",
    borderRadius: "8px",
    backgroundColor: "#4caf50",
    color: "#fff",
    border: "none",
    fontSize: "16px",
    cursor: "pointer",
  },
};
