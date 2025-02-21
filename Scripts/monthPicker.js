let selectedYear = new Date().getFullYear(); // Lấy năm hiện tại

function changeYear(offset) {
    selectedYear += offset;
    document.getElementById("selectedYear").innerText = selectedYear;
}

function selectMonth(month) {
    let formattedMonth = month.toString().padStart(2, '0'); // Chuyển thành 2 chữ số (01, 02,...)
    let formattedDate = `${selectedYear}-${formattedMonth}`; // Định dạng MM/YYYY
    document.getElementById("thangNam").value = formattedDate;

    // Lấy instance của modal và đóng nó
    let modalElement = document.getElementById("monthYearModal");
    let modalInstance = bootstrap.Modal.getInstance(modalElement);
    modalInstance.hide(); // Tắt modal ngay lập tức
}

// Hiển thị năm hiện tại khi tải trang
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("selectedYear").innerText = selectedYear;
});