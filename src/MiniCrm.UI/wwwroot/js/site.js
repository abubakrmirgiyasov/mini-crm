$(document).ready(function () {
    $("#statusSortSelect").on("change", function (e) {
        window.location.replace("/?sortOrder=" + this.value);
    });
});