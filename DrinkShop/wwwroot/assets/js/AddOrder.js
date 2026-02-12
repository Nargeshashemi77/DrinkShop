{
    const closeButton = document.getElementById('closeButton');
    const messageBox = document.getElementById('messageBox');
    const IdentityElement = document.getElementById("UserIdentityName");
    const thisUrl = window.location.href;
    const messageBoxText = document.getElementById("message-box-text");

    let returnUrl;

    messageBox.style.Color = "white";

    function showMessage() {
        messageBox.classList.add('show');
        setTimeout(hideMessage, 5000);
    }
    function hideMessage() {
        messageBox.classList.remove('show');
    }
    closeButton.addEventListener('click', function () {
        hideMessage();
    });
    document.getElementById("addToCart-submit-btn").addEventListener("click", async () => {
        try {
            if (IdentityElement.innerText == "") {
                const currentUrl = window.location.href;
                const urlObject = new URL(currentUrl);

                returnUrl = thisUrl.replace(urlObject.origin, "");

                window.location.href = urlObject.origin + `/Account/Login?message=firstLoginToYourAccount&returnUrl=${returnUrl}`;
            }
            else {
                const productIdElement = document.getElementById("productIdSection");
               
                const requestBody = {
                    productId: productIdElement.innerText,
                }
                console.log("Request Body: ", requestBody);
                const response = await fetch("/Indent/AddOrder", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(requestBody)
                });
                console.log("response: ", response);

                if (response.status === 400) {
                    messageBox.style.backgroundColor = "#ef2f2f";
                    messageBox.style.border = "2px solid #ef2f2f";
                    if (closeButton.classList.contains("success"))
                        closeButton.classList.remove("success");
                    closeButton.classList.add("error");
                    messageBoxText.innerText = "تعداد کالای وارد شده صحیح نیست";
                    showMessage();
                }
                else if (response.status === 404)
                    window.location.href = response.url;

                else if (response.status === 500) {
                    messageBox.style.backgroundColor = "#ef2f2f";
                    messageBox.style.backgroundColor = "#ef2f2f";
                    if (closeButton.classList.contains("success"))
                        closeButton.classList.remove("success");
                    closeButton.classList.add("error");
                    messageBoxText.innerText = "مشکلی پیش آمده است لطفا بعدا تلاش کنید";
                    showMessage();
                }
                else if (response.status === 200) {
                    const data = await response.json();
                    console.log("response: ", data);
                    if (data.message === "More than stock") {
                        messageBox.style.backgroundColor = "#ef2f2f";
                        messageBox.style.border = "2px solid #ef2f2f";
                        if (closeButton.classList.contains("success"))
                            closeButton.classList.remove("success");
                        closeButton.classList.add("error");
                        messageBoxText.innerText = "موجودی کافی نیست";
                        showMessage();
                    }
                    else if (data.message === "The Product Added already") {
                        messageBox.style.backgroundColor = "#ef2f2f";
                        messageBox.style.border = "2px solid #ef2f2f";
                        if (closeButton.classList.contains("success"))
                            closeButton.classList.remove("success");
                        closeButton.classList.add("error");
                        messageBoxText.innerText = "محصول از قبل به عنوان سفارش ثبت شده است";
                        showMessage();
                    }
                    else if (data.message === "Should complete user information first") {
                        window.location.href = data.url;
                    }
                    else if (data.message === "The product is not available") {
                        messageBox.style.backgroundColor = "#ef2f2f";
                        messageBox.style.border = "2px solid #ef2f2f";
                        if (closeButton.classList.contains("success"))
                            closeButton.classList.remove("success");
                        closeButton.classList.add("error");
                        messageBoxText.innerText = "محصول موجود نیست";
                        showMessage();
                    }
                    else if (data.message === "Success") {
                        messageBox.style.backgroundColor = "#079e1b";
                        messageBox.style.border = "2px solid #079e1b";
                        if (closeButton.classList.contains("error"))
                            closeButton.classList.remove("error");
                        closeButton.classList.add("success");
                        messageBoxText.innerText = "محصول با موفقیت به سفارشات شما افزوده شد. در حال انتقال به صفحه لیست سفارشات ...";
                        showMessage();
                        setTimeout(() => {
                            window.location.replace("/Indent/UserOrders");
                        }, 4000)
                    }
                }
            }
        }
        catch (err) {
            console.log("catched error: ", err.message);
            messageBox.style.backgroundColor = "#ef2f2f";
            messageBox.style.backgroundColor = "#ef2f2f";
            if (closeButton.classList.contains("success"))
                closeButton.classList.remove("success");
            closeButton.classList.add("error");
            messageBoxText.innerText = "مشکلی پیش آمده است لطفا بعدا تلاش کنید";
            showMessage();
        }
    });
}