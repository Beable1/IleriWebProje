
// Chat widget aç/kapat
function toggleChat() {
    fetchMessages
    const chatWidget = document.getElementById('chat-widget');
    chatWidget.classList.toggle('closed');

    // Toggle icon değişikliği
    const toggleIcon = chatWidget.querySelector('.chat-toggle-icon');
    toggleIcon.textContent = chatWidget.classList.contains('closed') ? '+' : '-';
    
}


async function fetchMessages() {
    try {
        const response = await fetch('/Chat/Index', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const messages = await response.json();
            console.log('Gelen Mesajlar:', messages);
        } else {
            console.error('Mesajlar alınırken hata oluştu:', response.statusText);
        }
    } catch (error) {
        console.error('Hata:', error);
    }
}





document.getElementById('chat-form').addEventListener('submit', async function (event) {
    event.preventDefault(); // Formun varsayılan submit davranışını engelle

    const messageInput = document.getElementById('chat-message-input');
    const messageContent = messageInput.value.trim();

    if (!messageContent) {
        alert("Mesaj boş olamaz!");
        return;
    }

    try {
        // API'ye POST isteği gönder
        const response = await fetch('/Chat/SendMessage', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ content: messageContent }) // Sunucuya JSON formatında gönderiyoruz
        });

        if (!response.ok) {
            throw new Error('Mesaj gönderilirken bir hata oluştu.');
        }

        // Sunucudan dönen cevabı al
        const result = await response.json();
        console.log('Sunucudan Gelen Cevap:', result);

        // Mesajı UI'ye ekle
        addMessageToUI('You', messageContent);
        addMessageToUI('Server', result.message);

        // Mesaj giriş alanını temizle
        messageInput.value = '';
    } catch (error) {
        console.error('Hata:', error);
        alert('Mesaj gönderilemedi.');
    }
});

// Gelen mesajları UI'ye ekleme fonksiyonu
function addMessageToUI(sender, content) {
    const chatMessages = document.getElementById('chat-messages');
    const messageElement = document.createElement('div');
    messageElement.textContent = `${sender}: ${content}`;
    chatMessages.appendChild(messageElement);
    chatMessages.scrollTop = chatMessages.scrollHeight;
}
