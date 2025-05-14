document.addEventListener('DOMContentLoaded', function () {
    // Dava tablosundaki satırlara tıklama eventi
    const tableRows = document.querySelectorAll('#cases-table tbody tr');
    tableRows.forEach(row => {
        row.addEventListener('click', function () {
            const caseId = this.getAttribute('data-case-id');
            loadCaseDetails(caseId);
        });
    });

    // Duruşma bilgilerini göster/gizle butonu
    const showHearingsBtn = document.getElementById('showHearingsBtn');
    if (showHearingsBtn) {
        showHearingsBtn.addEventListener('click', toggleHearingDetails);
    }

    // Örnek dava detayı yükleme fonksiyonu
    function loadCaseDetails(caseId) {
        // AJAX ile sunucudan veri çekme simülasyonu
        fetchCaseData(caseId).then(caseData => {
            // Dava detaylarını doldur
            document.getElementById('case-details').style.display = 'block';
            document.querySelector('.case-title').textContent = caseData.title;
            document.querySelector('.case-id').textContent = `Dava No: ${caseData.caseNumber}`;
            document.querySelector('.status-active').textContent = caseData.status;

            // Diğer alanları doldur...

            // Duruşma bilgilerini gizle
            document.getElementById('hearing-details').style.display = 'none';
            showHearingsBtn.innerHTML = '<i class="fas fa-balance-scale"></i> Duruşma Bilgilerini Göster';
        });
    }

    // Duruşma detaylarını açıp kapama
    function toggleHearingDetails() {
        const hearingDetails = document.getElementById('hearing-details');
        if (hearingDetails.style.display === 'none') {
            loadHearingDetails(this.getAttribute('data-case-id'));
            hearingDetails.style.display = 'block';
            this.innerHTML = '<i class="fas fa-balance-scale"></i> Duruşma Bilgilerini Gizle';
        } else {
            hearingDetails.style.display = 'none';
            this.innerHTML = '<i class="fas fa-balance-scale"></i> Duruşma Bilgilerini Göster';
        }
    }

    // Duruşma detaylarını yükleme fonksiyonu
    function loadHearingDetails(caseId) {
        // AJAX ile sunucudan duruşma bilgilerini çekme simülasyonu
        fetchHearingData(caseId).then(hearingData => {
            document.getElementById('detail-davaNo').textContent = hearingData.caseNumber;
            document.getElementById('detail-konu').textContent = hearingData.subject;
            document.getElementById('detail-date').textContent = hearingData.date;
            document.getElementById('detail-time').textContent = hearingData.time;
            document.getElementById('detail-court').textContent = hearingData.court;

            // Durum bilgisini güncelle
            const statusElement = document.getElementById('detail-status');
            statusElement.textContent = hearingData.status;
            statusElement.className = 'status'; // Önceki classları temizle
            statusElement.classList.add(`status-${hearingData.statusClass}`);
        });
    }

    // Örnek API çağrıları (Gerçek uygulamada fetch/axios kullanılacak)
    function fetchCaseData(caseId) {
        // Burada gerçek bir API çağrısı yapılacak
        return new Promise(resolve => {
            const sampleData = {
                id: caseId,
                title: 'Boşanma Davası',
                caseNumber: '#2023-1254',
                status: 'Aktif',
                // Diğer alanlar...
            };
            resolve(sampleData);
        });
    }

    function fetchHearingData(caseId) {
        // Burada gerçek bir API çağrısı yapılacak
        return new Promise(resolve => {
            const sampleData = {
                caseNumber: '#2023-1254',
                subject: 'Boşanma Davası - İlk Duruşma',
                date: '10.06.2023',
                time: '10:00',
                court: 'İstanbul 3. Aile Mahkemesi',
                status: 'Bekliyor',
                statusClass: 'pending'
            };
            resolve(sampleData);
        });
    }
});