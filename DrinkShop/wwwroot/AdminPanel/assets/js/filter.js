{
    const selectfilterElement = document.getElementById("OrderFilterSelect");
    const periodSelectElement = document.getElementById("periodSelect");
    const currentUrl = window.location.href;
    const urlObject = new URL(currentUrl);
    const searchParams = urlObject.searchParams;

    let selectedfilterOptionValue, selectedPeriodOptionValue, updatedUrl, filterParam = "orderStatus", periodParam = "period";

    if (searchParams.has(filterParam)) {
        let paramValue = searchParams.get(filterParam);
        let optionElement;
        switch (paramValue) {
            case "registered":
                optionElement = getOptionByValue("registered", selectfilterElement);
                break;
            case "pending":
                optionElement = getOptionByValue("pending", selectfilterElement);
                break;
            case "finished":
                optionElement = getOptionByValue("finished", selectfilterElement);
                break;
            case "canceled":
                optionElement = getOptionByValue("canceled", selectfilterElement);
                break;
            case "doing":
                optionElement = getOptionByValue("doing", selectfilterElement);
                break;
            case "rejected":
                optionElement = getOptionByValue("rejected", selectfilterElement);
                break;
            case "reffered":
                optionElement = getOptionByValue("reffered", selectfilterElement);
                break;
            default:
                optionElement = getOptionByValue("registered", selectfilterElement);
        }

        //selectfilterElement.children[0].removeAttribute("selected");
        optionElement.setAttribute("selected", "");
    }

    if (searchParams.has(periodParam)) {
        let paramValue = searchParams.get(periodParam);
        let optionElement;
        switch (paramValue) {
            case "Today":
                optionElement = getOptionByValue("Today", periodSelectElement);
                break;
            case "Weekly":
                optionElement = getOptionByValue("Weekly", periodSelectElement);
                break;
            case "Monthly":
                optionElement = getOptionByValue("Monthly", periodSelectElement);
                break;
            case "Yearly":
                optionElement = getOptionByValue("Yearly", periodSelectElement);
                break;
        }

        //periodSelectElement.children[0].removeAttribute("selected");
        optionElement.setAttribute("selected", "");
    }
    selectfilterElement.addEventListener("change", function () {

        selectedfilterOptionValue = selectfilterElement.value;

        if (searchParams.has(filterParam)) {
            searchParams.set(filterParam, selectedfilterOptionValue);
        }
        else
            searchParams.append(filterParam, selectedfilterOptionValue);

        updatedUrl = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
        window.location.href = updatedUrl;
    });

    periodSelectElement.addEventListener("change", () => {
        selectedPeriodOptionValue = periodSelectElement.value;

        if (searchParams.has(periodParam)) {
            searchParams.set(periodParam, selectedPeriodOptionValue);
        }
        else
            searchParams.append(periodParam, selectedPeriodOptionValue);

        updatedUrl = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
        window.location.href = updatedUrl;
    });
    function getOptionByValue(value, selectElement) {
        for (const option of selectElement.options) {
            if (option.value === value) {
                return option;
            }
        }
    }
}