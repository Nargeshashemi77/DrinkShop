{
    const currentUrl = window.location.href;
    const urlObject = new URL(currentUrl);
    const searchParams = urlObject.searchParams;
    const selectLimitElement = document.getElementById("SelectLimitOfPage");
    const ElementThatContainPages = document.getElementById("pagesContainer");
    const currentPageElement = document.getElementById("activePage");
    const currentPageInNumber = parseInt(currentPageElement.innerText);
    const pagesCountInNumber = parseInt(document.getElementById("pagesCount").innerText);

    let url, pageParam = "page", limitParam = "limit";
    let i, selectLimitElementValue;

    if (searchParams.has(limitParam)) {
        let paramValue = searchParams.get(limitParam);
        let optionElement;
        switch (paramValue) {
            case "21":
                optionElement = getOptionByValue("21");
                break;
            case "33":
                optionElement = getOptionByValue("33");
                break;
            case "42":
                optionElement = getOptionByValue("42");
                break;
            default:
                optionElement = getOptionByValue("9");
        }
        selectLimitElement.children[0].removeAttribute("selected");
        optionElement.setAttribute("selected", "");
    }
    {
        const currentUrl = window.location.href;
        const urlObject = new URL(currentUrl);
        const searchParams = urlObject.searchParams;

        let aElement;

        for (i = 0; i < ElementThatContainPages.childElementCount; i++) {

            aElement = ElementThatContainPages.children[i]


            if (!isNaN(aElement.innerText)) {

                if (searchParams.has(pageParam))
                    searchParams.set(pageParam, aElement.innerText);
                else
                    searchParams.append(pageParam, aElement.innerText);

                url = urlObject.origin + urlObject.pathname + "?" + searchParams.toString()

                aElement.setAttribute("href", url);
            }

        }
    }
    selectLimitElement.addEventListener("change", function () {
        selectLimitElementValue = selectLimitElement.value;

        if (searchParams.has(limitParam)) searchParams.set(limitParam, selectLimitElementValue);
        else searchParams.append(limitParam, selectLimitElementValue);

        url = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
        window.location.href = url;
    });
    function previousPage() {
        if (currentPageInNumber != 1) {
            if (searchParams.has(pageParam))
                searchParams.set(pageParam, currentPageInNumber - 1);
            else
                searchParams.append(pageParam, currentPageInNumber - 1);

            url = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();

            window.location.href = url;
        }
    }
    function nextpage() {
        if (currentPageInNumber != pagesCountInNumber) {
            if (searchParams.has(pageParam))
                searchParams.set(pageParam, currentPageInNumber + 1);
            else
                searchParams.append(pageParam, currentPageInNumber + 1);

            url = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
            window.location.href = url;
        }
    }
    function getOptionByValue(value) {
        for (const option of selectLimitElement.options) {
            if (option.value === value) {
                return option;
            }
        }
    }
}
