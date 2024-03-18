document.addEventListener("DOMContentLoaded", function () {
    var accountRows = document.querySelectorAll("#accountTable tr[data-account-id]");
    var editLink = document.querySelector(".editLink");
    var detailsLink = document.querySelector(".detailsLink");
    var deleteLink = document.querySelector(".deleteLink");

    editLink.classList.add("disabled");
    deleteLink.classList.add("disabled");

    editLink.addEventListener("click", function (event) {
        if (editLink.classList.contains("disabled")) {
            event.preventDefault();
        }
    });

    deleteLink.addEventListener("click", function (event) {
        if (deleteLink.classList.contains("disabled")) {
            event.preventDefault();
        }
    });

    accountRows.forEach(function (row) {
        var accountId = row.getAttribute("data-account-id");
        row.addEventListener("click", function () {
            var accountInfoLinks = document.querySelectorAll(".accountInfo");
            var prevSelectedRow = document.querySelector("#accountTable tr.selected");

            if (prevSelectedRow) {
                prevSelectedRow.classList.remove("selected");
            }

            if (accountId === "1") {
                detailsLink.classList.remove("disabled");
                editLink.classList.add("disabled");
                deleteLink.classList.add("disabled");
            } else {
                detailsLink.classList.remove("disabled");
                editLink.classList.remove("disabled");
                deleteLink.classList.remove("disabled");
            }

            accountInfoLinks.forEach(function (link) {
                link.href = link.href.replace("__id__", accountId);
            });

            row.classList.add("selected");
        });
    });
});