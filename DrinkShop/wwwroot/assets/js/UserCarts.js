{
    function editCart(cartId) {
        const numberInput = document.getElementById(`cartId_${cartId}`);

        if (!numberInput) {
            console.error(`فیلد تعداد برای آیتم با شناسه سبد خرید ${cartId} یافت نشد`);
            return;
        }

        const quantity = numberInput.value;

        updateCartItem(cartId, quantity)
            .then(data => {



                if (data.message === "Update successfully") {
                    setTimeout(() => {
                        window.location.replace("/carts");
                    }, 2000)
                }
            })
            .catch(err => console.error(err))
    }

    function deleteCartItem(cartId) {
        deleteItem(cartId);
    }

    async function updateCartItem(cartId, quantity) {
        const response = await fetch(`/carts/items/${cartId}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                number: quantity
            })
        });

        if (!response.ok) throw new Error("Request failed");

        return await response.json();
    }

    async function deleteItem(cartId) {
        const response = await fetch(`/carts/items/${cartId}`, {
            method: "DELETE",
        });

        if (response.ok) {
            setTimeout(() => {
                window.location.replace("/carts");
            }, 2000)
        }
    }
}