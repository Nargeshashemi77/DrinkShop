{
    let OrderDescription;
    const userOrdersTable = document.getElementById("userOrdersTable");
    function editOrderRequest(orderId) {

        //console.log("user Orders Table Body: ", userOrdersTable);
        //console.log("user Orders Table Body Children Count: ", userOrdersTable.childElementCount);

        for (let i = 1; i < userOrdersTable.childElementCount; i++) {

            //console.log("i: ", i);
            //console.log(`Row Order Id: `, userOrdersTable.children[i].children[0].children[0].innerHTML);
            //console.log(`Row Description: `, userOrdersTable.children[i].children[0].children[7].children[0].innerHTML);
            //console.log("Order Id: ", orderId);

            if (userOrdersTable.children[i].children[0].children[0].innerHTML == orderId)
                OrderDescription = userOrdersTable.children[i].children[0].children[7].children[0].value;

            //console.log(`user Orders Table.children[${i}].children[0].children[0].innerHTML === ${orderId}`);
            //console.log("Description: ", userOrdersTable.children[i].children[0].children[7].children[0].value);
        }
        fetch("/Indent/EditOrder", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                OrderId: orderId,
                Description: OrderDescription
            })
        });
    }
}