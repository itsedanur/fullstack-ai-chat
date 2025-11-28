# fullstack-ai-chat
# fullstack-ai-chat


# 🤖 Full Stack + AI Chat – Duygu Analizi Projesi

## 🎯 Proje Özeti

Bu proje, kullanıcıların mesajlaşabildiği ve yazışmaların **AI (Yapay Zeka)** tarafından analiz edilerek **duygu durumunun (pozitif, nötr, negatif)** anında gösterildiği tam yığın (Full Stack) bir sohbet uygulamasıdır.
Proje web (React), mobil (React Native CLI), backend (.NET Core) ve AI servisini (Python + Gradio) uçtan uca entegre eder.

---

## ⚙️ Teknolojiler

| Katman                   | Teknoloji          | Hosting Servisi                                   |
| ------------------------ | ------------------ | ------------------------------------------------- |
| **Frontend (Web)** | React              | [Vercel](https://vercel.com)                         |
| **Backend (API)**  | .NET Core + SQLite | [Render](https://render.com)                         |
| **AI Servisi**     | Python + Gradio    | [Hugging Face Spaces](https://huggingface.co/spaces) |
| **Mobil**          | React Native CLI   | Xcode iOS Simülatör / Android Build             |

---

## 🧩 Klasör Yapısı

/frontend → React web uygulaması (Vercel'e deploy edildi)
/backend → .NET Core Web API (Render'da canlı)
/ai-service → Python Gradio AI servisi (Hugging Face Spaces'ta canlı)


---
## 🌐 Canlı Demo Linkleri

- 💻 **Web (React + Vercel):**  
  👉 [https://fullstack-ai-chat-six.vercel.app](https://fullstack-ai-chat-six.vercel.app)

- ⚙️ **Backend (Render .NET API):**  
  👉 [https://sentimentchatapi.onrender.com/api/messages](https://sentimentchatapi.onrender.com/api/messages)

- 🤖 **AI Servisi (Hugging Face Spaces):**  
  👉 [https://edanurunal-sentiment-analysis.hf.space](https://edanurunal-sentiment-analysis.hf.space)
---
## 💬 Kullanım Adımları

1. Web veya mobil arayüzde mesajınızı yazın.
2. Mesaj API üzerinden AI servisine gönderilir.
3. AI, mesajı analiz eder ve sonucu “Pozitif 😊”, “Nötr 🙂” veya “Negatif 😞” olarak döndürür.
4. Sonuç, anında kullanıcı arayüzünde görünür.


# Frontend (React Native)

cd frontend
npx react-native run-ios

# Backend (C# API)

cd backend/SentimentApi
dotnet run --urls "http://0.0.0.0:5252"


---

## 📱 Mobil Görünüm (React Native CLI)

Uygulama iOS simülatöründe test edilmiştir. Android cihazlar için `npx react-native run-android` ile build alınabilir.

Örnek ekran görüntüsü

<img width="1206" height="2622" alt="Simulator Screenshot - iPhone 17 Pro - 2025-11-12 at 22 14 52" src="https://github.com/user-attachments/assets/2385ab67-de34-444b-98d3-7e50fd87fb8b" />

 


<p align="center">
  <img src="image/README/1762975021287.png" alt="Mobil Görünüm" width="350"/>
</p>



> 📷 Yukarıdaki ekran görüntüsü iOS simülatöründen alınmıştır.
> Kullanıcı mesaj yazdığında AI tarafından analiz edilir ve sonuç anında görüntülenir.
>

---

## 🧠 Öğrenilenler

- React, .NET Core ve Python AI servislerinin entegrasyonu
- API çağrıları, HTTP istekleri, JSON veri işleme
- Ücretsiz servislerde (Render, Hugging Face, Vercel) uçtan uca deploy işlemi
- React Native ile mobil uyumlu arayüz geliştirme

---

## 🧑‍💻 Katkıda Bulunanlar

- **Eda Nur Ünal** — Full Stack Developer
  📧 edanurunal562@gmail.com
  🌐 [GitHub: itsedanur](https://github.com/itsedanur)

---

## 📜 Lisans

Bu proje MIT lisansı ile paylaşılmıştır. Dilediğiniz gibi inceleyebilir, referans göstermek koşuluyla geliştirmelerde kullanabilirsiniz.
