document.addEventListener("DOMContentLoaded", function () {
    var accountRows = document.querySelectorAll("#accountTable tr[data-account-id]");
    var transaction = document.querySelector(".transaction");
    var deleteLink = document.querySelector(".deleteLink");

    transaction.addEventListener("click", function (event) {
        if (transaction.classList.contains("disabled")) {
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

            if (transaction && deleteLink) {
                var prevSelectedRow = document.querySelector("#accountTable tr.selected");

                if (prevSelectedRow) {
                    prevSelectedRow.classList.remove("selected");
                }

                if (accountId === "1") {
                    transaction.classList.add("disabled");
                    deleteLink.classList.add("disabled");
                } else {
                    transaction.classList.remove("disabled");
                    deleteLink.classList.remove("disabled");
                }

                accountInfoLinks.forEach(function (link) {
                    link.href = link.href.replace("__id__", accountId);
                });

                row.classList.add("selected");
            }
        });
    });
});