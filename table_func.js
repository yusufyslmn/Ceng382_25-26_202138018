document.addEventListener("DOMContentLoaded", function () {
    document.body.classList.add("fade-in");
    const form = document.getElementById("classForm");
    const table = document.getElementById("classTable");
    const tableBody = table.getElementsByTagName("tbody")[0];
    const submitButton = document.getElementById("submit");

    // Form submission event
    form.addEventListener("submit", function (event) {
        event.preventDefault();

        const className = document.getElementById("className").value;
        const numPeople = document.getElementById("numPeople").value;
        const description = document.getElementById("description").value;

        const newRow = tableBody.insertRow();
        newRow.insertCell(0).textContent = className;
        newRow.insertCell(1).textContent = numPeople;
        newRow.insertCell(2).textContent = description;

        // Click event: Log row details and highlight the row
        newRow.addEventListener("click", function () {
            console.log(`Class: ${className}, People: ${numPeople}, Description: ${description}`);
            newRow.classList.toggle("highlight");
        });

        // Mouseover & Mouseout event for row
        newRow.addEventListener("mouseover", function () {
            this.style.backgroundColor = "#4682B4";
        });

        newRow.addEventListener("mouseout", function () {
            this.style.backgroundColor = "";
        });

        // Double-click event to remove row
        newRow.addEventListener("dblclick", function () {
            this.remove();
            console.log("Row removed");
        });

        form.reset();
    });

    // Input focus and blur events
    document.querySelectorAll("input, textarea").forEach(input => {
        input.addEventListener("focus", function () {
            this.style.background = "#c9c9c9";
        });

        input.addEventListener("blur", function () {
            if (this.value.trim() === "") {
                this.style.border = "2px solid steelblue";
            } else {
                this.style.border = "";
            }
        });
    });
});