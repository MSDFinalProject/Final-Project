function CanadaVal() {
    if (document.getElementById("inputRegion").value == "Canada") {
        document.getElementById("inputState").value = "";
        document.getElementById("inputState").disabled = true;
    }
    else {
        document.getElementById("inputState").disabled = false;
    }
}

function PriorityVal() {
    if (document.getElementById("inputPriority").value == "High") {
        document.getElementById("inputPriorityReason").disabled = false;
    }
    else {
        document.getElementById("inputPriorityReason").disabled = true;
        document.getElementById("inputPriorityReason").value = "";
    }
}

function SSNVal() {
    if (!/^[0-9]+$/.test(document.querySelector("#inputSSN").value)) {
        document.querySelector("#inputSSN").value = "";
        document.querySelector("#inputSSN").setCustomValidity("SSN must be numeric.");
    }
    else {
        document.querySelector("#inputSSN").setCustomValidity("");
    }
}

function NameVal() {
    if (/^[0-9]+$/.test(document.querySelector("#inputFirstName").value)) {
        document.querySelector("#inputFirstName").value = "";
        document.querySelector("#inputFirstName").setCustomValidity("Name must only contain letters.");
    }
    else {
        document.querySelector("#inputFirstName").setCustomValidity("");
    }

    if (/^[0-9]+$/.test(document.querySelector("#inputLastName").value)) {
        document.querySelector("#inputLastName").value = "";
        document.querySelector("#inputLastName").setCustomValidity("Name must only contain letters.");
    }
    else {
        document.querySelector("#inputLastName").setCustomValidity("");
    }
}

function ZipVal() {
    if (!/^[0-9]+$/.test(document.querySelector("#inputZip").value)) {
        document.querySelector("#inputZip").value = "";
        document.querySelector("#inputZip").setCustomValidity("Zip Code must be numeric.");
    }
    else {
        document.querySelector("#inputZip").setCustomValidity("");
    }
}

function MortgageAmountVal() {
    if (!/^[0-9]+$/.test(document.querySelector("#inputMortgageAmount").value)) {
        document.querySelector("#inputMortgageAmount").value = "";
        document.querySelector("#inputMortgageAmount").setCustomValidity("Mortgage Amount must be numeric.");
    }
    else {
        document.querySelector("#inputMortgageAmount").setCustomValidity("");
    }

    if (document.querySelector("#inputMortgageAmount").value < 50000){
        document.querySelector("#inputMortgageAmount").setCustomValidity("Mortgage Amount must be greater than $50,000.");
    }
    else {
        document.querySelector("#inputMortgageAmount").setCustomValidity("");
    }
}
