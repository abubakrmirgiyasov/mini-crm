$(document).ready(function () {
    $("#statusSortSelect").on("change", function (e) {
        window.location.replace("/Project/?sortOrder=" + this.value);
    });

    $("#statusTaskSortSelect").on("change", function (e) {
        window.location.replace("/Task/?sortOrder" + this.value);
    });
});