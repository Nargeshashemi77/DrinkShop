{
    function editOrderRequest(orderId) {
        const numberInput = document.getElementById(`orderNumber_${orderId}`);

        if (!numberInput) {
            console.error(`فیلد تعداد برای سفارش ${orderId} یافت نشد`);
            return;
        }

        const orderNumber = numberInput.value;

        fetch("/Indent/EditOrder", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                OrderId: orderId,
                number: orderNumber
            })
        });
    }
}