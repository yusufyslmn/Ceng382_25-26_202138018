document.getElementById("loginBtn").addEventListener("click", function() {
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;

    if (username === "admin" && password === "admin") {
        window.location.href = "table.html"; // Giriş başarılıysa yönlendirme
    } else {
        alert("Invalid username or password!"); // Hatalı giriş uyarısı
    }
});
