document.addEventListener("DOMContentLoaded", function () {
    // Duruşma satırlarına tıklama olayı ekle
    document.querySelectorAll(".hearings-table tbody tr").forEach(function (row) {
        row.addEventListener("click", function () {
            // Tüm satırlardan "selected-row" sınıfını kaldır
            document.querySelectorAll(".hearings-table tbody tr").forEach(r => r.classList.remove("selected-row"));

            // Bu satıra "selected-row" sınıfını ekle
            this.classList.add("selected-row");

            // Bu satırdaki hücreleri al
            var cells = this.querySelectorAll("td");

            // Detayları almak
            var davaNo = cells[0].textContent;
            var konu = cells[1].textContent;
            var tarih = cells[2].textContent;
            var saat = cells[3].textContent;
            var mahkeme = cells[4].textContent;
            var durum = cells[5].textContent;
            var durumClass = cells[5].querySelector("span").className;

            // Detayları yerleştir
            document.getElementById("detail-davaNo").textContent = davaNo;
            document.getElementById("detail-konu").textContent = konu;
            document.getElementById("detail-date").textContent = tarih;
            document.getElementById("detail-time").textContent = saat;
            document.getElementById("detail-court").textContent = mahkeme;
            document.getElementById("detail-status").textContent = durum;
            document.getElementById("detail-status").className = durumClass;

            // Detayları göster
            document.getElementById("hearing-details").style.display = "block";
        });
    });
});
