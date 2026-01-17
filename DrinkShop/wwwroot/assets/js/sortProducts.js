{
    const selectSortOptionElement = document.getElementById("SortProduct");
    const FilterCheckBoxElement = document.getElementById("filterCheckbox");
    const currentUrl = window.location.href;
    const urlObject = new URL(currentUrl);
    const searchParams = urlObject.searchParams;

    let selectedSortOptionValue, updatedUrl, sortParam = "sort", filterParam = "filter";
    selectedSortOptionValue = selectSortOptionElement.value;

    if (searchParams.has(sortParam)) {
        let paramValue = searchParams.get(sortParam);
        let optionElement;
        switch (paramValue) {
            case "Newest":
                optionElement = getOptionByValue("Newest", selectSortOptionElement);
                break;
            case "Oldest":
                optionElement = getOptionByValue("Oldest", selectSortOptionElement);
                break;
            case "ExpensiveToCheap":
                optionElement = getOptionByValue("ExpensiveToCheap", selectSortOptionElement);
                break;
            case "CheapToExpensive":
                optionElement = getOptionByValue("CheapToExpensive", selectSortOptionElement);
                break;
            case "AlphabetAscending":
                optionElement = getOptionByValue("AlphabetAscending", selectSortOptionElement);
                break;
            case "AlphabetDescending":
                optionElement = getOptionByValue("AlphabetDescending", selectSortOptionElement);
                break;
            default:
                optionElement = getOptionByValue("null");
        }
        selectSortOptionElement.children[0].removeAttribute("selected");
        optionElement.setAttribute("selected", "");
    }

    if (searchParams.has(filterParam)) {
        let paramValue = searchParams.get(filterParam);
        if (paramValue === "available")
            FilterCheckBoxElement.checked = true;
    }
    selectSortOptionElement.addEventListener("change", function () {

        selectedSortOptionValue = selectSortOptionElement.value;

        if (searchParams.has(sortParam)) {
            if (selectedSortOptionValue === "null")
                searchParams.delete(sortParam)

            else
                searchParams.set(sortParam, selectedSortOptionValue);
        }
        else {
            if (selectedSortOptionValue !== "null")
                searchParams.append(sortParam, selectedSortOptionValue);
        }

        updatedUrl = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
        window.location.href = updatedUrl;
    });

    FilterCheckBoxElement.addEventListener("click", () => {
        if (FilterCheckBoxElement.checked) {
            if (searchParams.has(filterParam)) searchParams.set(filterParam, "available");
            else searchParams.append(filterParam, "available");
        }
        else {
            if (searchParams.has(filterParam))
                searchParams.delete(filterParam);
        }
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