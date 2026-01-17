{
    const currentUrl = window.location.href;
    const urlObject = new URL(currentUrl);
    const searchParams = urlObject.searchParams;
    const searchBtn = document.getElementById("search_button");
    const searchInput = document.getElementById("search_input");
    const searchParam = "search";

    if (searchParams.has(searchParam)) {
        let paramValue = searchParams.get(searchParam);
        searchInput.value = paramValue;
    }

    searchBtn.addEventListener("click", () => {
        if (searchParams.has(searchParam)) {
            if (searchInput.value === "")
                searchParams.delete(searchParam);
            else
                searchParams.set(searchParam, searchInput.value);
        }
        else {
            if (searchInput.value !== "")
                searchParams.append(searchParam, searchInput.value);
        }
        if (searchParams.toString() !== "")
            updatedUrl = urlObject.origin + urlObject.pathname + "?" + searchParams.toString();
        else
            updatedUrl = urlObject.origin + urlObject.pathname;
        window.location.href = updatedUrl;
    });

}