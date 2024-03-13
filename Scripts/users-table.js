document.addEventListener("DOMContentLoaded", function () {
    var userRows = document.querySelectorAll("#userTable tr[data-user-id]");
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

    userRows.forEach(function (row) {
        row.addEventListener("click", function () {
            var userId = row.getAttribute("data-user-id");
            var userInfoLinks = document.querySelectorAll(".userInfo");
            var prevSelectedRow = document.querySelector("#userTable tr.selected");

            if (prevSelectedRow) {
                prevSelectedRow.classList.remove("selected");
            }

            if (userId === "1") {
                detailsLink.classList.remove("disabled");
                editLink.classList.add("disabled");
                deleteLink.classList.add("disabled");
            } else {
                detailsLink.classList.remove("disabled");
                editLink.classList.remove("disabled");
                deleteLink.classList.remove("disabled");
            }

            userInfoLinks.forEach(function (link) {
                link.href = link.href.replace("__id__", userId);
            });

            row.classList.add("selected");
        });
    });
});