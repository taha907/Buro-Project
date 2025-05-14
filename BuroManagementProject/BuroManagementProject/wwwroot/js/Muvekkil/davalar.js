document.addEventListener("DOMContentLoaded", function () {
    const rows = document.querySelectorAll(".cases-table tbody tr");
    const detailsPanel = document.getElementById("case-details");

    rows.forEach(function (row) {
        row.addEventListener("click", function () {
            // Diğer satırların seçimini kaldır
            rows.forEach(r => r.classList.remove("selected-row"));
            this.classList.add("selected-row");

            // Hücreleri al
            const cells = this.querySelectorAll("td");
            const davaNo = cells[0].textContent.trim();
            const konu = cells[1].textContent.trim();
            const avukat = cells[2].textContent.trim();
            const baslangicTarihi = cells[3].textContent.trim();
            const durumSpan = cells[4].querySelector("span");
            const durumText = durumSpan.textContent.trim();
            const durumClass = durumSpan.className;

            // Panel başlık bilgileri
            detailsPanel.querySelector(".case-title").textContent = konu;
            detailsPanel.querySelector(".case-id").textContent = "Dava No: " + davaNo;

            // Durum
            const statusElement = detailsPanel.querySelector(".case-header .status");
            statusElement.textContent = durumText;
            statusElement.className = "status " + durumClass.replace("status", "").trim();

            // Diğer bilgileri doldur
            const infoValues = detailsPanel.querySelectorAll(".info-value");
            // Avukat
            infoValues[0].textContent = avukat;
            // Dava Tarihi
            infoValues[1].textContent = baslangicTarihi;

            // Aşağıdaki alanlara tablo dışından veri gelmediği için boş bırakıyoruz
            infoValues[2].textContent = ""; // Mahkeme
            infoValues[3].textContent = ""; // Son Güncelleme
            infoValues[4].textContent = ""; // Açıklama

            // Ödeme alanlarını boş bırak
            const paymentValues = detailsPanel.querySelectorAll(".payment-value");
            paymentValues.forEach(p => p.textContent = "");

            // Paneli göster
            detailsPanel.style.display = "block";
        });
    });
});
