{
    const currentUrl = window.location.href;
    const urlObject = new URL(currentUrl);
    const searchParams = urlObject.searchParams;
    const SelectCategory = document.getElementById("dropdownCategory");
    const SelectSortBy = document.getElementById("dropdownSortBy");
    const SelectLimit = document.getElementById("dropdownLimit");
    const currentPageElement = document.getElementById("activePage");
    const currentPageInNumber = parseInt(currentPageElement.innerText);
    const pagesCountInNumber = parseInt(document.getElementById("pagesCount").innerText);

    let SelectCategoryValue, SelectSortValue, SelectLimitValue, SelectPageValue;
    let CategoryParam = "groupId", SortParam = "sort", limitParam = "limit", pageParam = "page";
    if (searchParams.has(CategoryParam)) {
        let paramValue = searchParams.get(CategoryParam);

        let optionElement = getOptionByValue(paramValue, SelectCategory);

        SelectCategory.children[0].removeAttribute("selected");

        optionElement.setAttribute("selected", "");
    }

    if (searchParams.has(SortParam)) {
        let paramValue = searchParams.get(SortParam);

        let optionElement = getOptionByValue(paramValue, SelectSortBy);

        SelectSortBy.children[0].removeAttribute("selected");

        optionElement.setAttribute("selected", "");
    }

    if (searchParams.has(limitParam)) {
        let paramValue = searchParams.get(limitParam);

        let optionElement = getOptionByValue(paramValue, SelectLimit);

        SelectSortBy.children[0].removeAttribute("selected");

        optionElement.setAttribute("selected", "");
    }

    SelectCategory.addEventListener("change", () => {
        SelectCategoryValue = SelectCategory.value;
        if (searchParams.has(CategoryParam)) searchParams.set(CategoryParam, SelectCategoryValue);
        else searchParams.append(CategoryParam, SelectCategoryValue);

        url = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
        window.location.href = url;
    });

    SelectSortBy.addEventListener("change", () => {
        SelectSortValue = SelectSortBy.value;
        if (searchParams.has(SortParam)) searchParams.set(SortParam, SelectSortValue);
        else searchParams.append(SortParam, SelectSortValue);

        url = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
        window.location.href = url;
    });

    SelectLimit.addEventListener("change", () => {
        SelectLimitValue = SelectLimit.value;
        if (searchParams.has(limitParam)) searchParams.set(limitParam, SelectLimitValue);
        else searchParams.append(limitParam, SelectLimitValue);

        url = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
        window.location.href = url;
    });

    function selectPage(button) {
        SelectPageValue = button.innerText;
        if (searchParams.has(pageParam)) searchParams.set(pageParam, SelectPageValue);
        else searchParams.append(pageParam, SelectPageValue);

        url = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
        window.location.href = url;
    }
    function previousPage() {
        console.log("previous page");
        if (currentPageInNumber != 1) {
            if (searchParams.has(pageParam))
                searchParams.set(pageParam, currentPageInNumber - 1);
            else
                searchParams.append(pageParam, currentPageInNumber - 1);

            url = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();

            window.location.href = url;
        }
    }
    function nextPage() {
        console.log("next page");
        if (currentPageInNumber != pagesCountInNumber) {
            if (searchParams.has(pageParam))
                searchParams.set(pageParam, currentPageInNumber + 1);
            else
                searchParams.append(pageParam, currentPageInNumber + 1);

            url = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
            window.location.href = url;
        }
    }
    function getOptionByValue(value, SelectElement) {
        for (const option of SelectElement.options) {
            if (option.value === value) {
                return option;
            }
        }
    }
}