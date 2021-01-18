$(document).ready(function () {
    var host = "https://" + window.location.host;
    var token = null;
    var headers = {};
    var productsEndpoint = "/api/products";
    var editId;
    

    //pripremanje dogadjaja
    $("body").on("click", "#btnDelete", deleteProducts);
    $("body").on("click", "#btnEdit", editProducts);




    loadProducts();

    //funkcija ucitavanja proizvoda
    function loadProducts() {
        var requestUrl = host + productsEndpoint;
        $.getJSON(requestUrl, setProducts);
    }

    //popunjavanje tabele proizvoda
    function setProducts(data, status) {

        var $container = $("#data");
        $container.empty();

        if (status === "success") {
            // ispis naslova
            var div = $("<div></div>");
            var h1 = $("<h1>Products</h1>");
            div.append(h1);
            // ispis tabele
            var table = $("<table class='table table-bordered'></table>");
            var header;
            if (token) {
                header = $("<thead><tr style='background-color:cornflowerblue'><td>Id</td><td>Name</td><td>Price</td><td>Production year</td><td>Option</td><td>Option</td></tr></thead>");
            } else {
                header = $("<thead><tr style='background-color:cornflowerblue'><td>Id</td><td>Name</td><td>Price</td><td>Production year</td></tr></thead>");
            }

            table.append(header);
            tbody = $("<tbody></tbody>");
            for (i = 0; i < data.length; i++) {
                // prikazujemo novi red u tabeli
                var row = "<tr>";
                // prikaz podataka
                var displayData = "<td>" + data[i].Id + "</td><td>" + data[i].Name + "</td><td>" + data[i].Price + "</td><td>" + data[i].ProductionYear + "</td>";
                
                // prikaz dugmadi za izmenu i brisanje
                var stringId = data[i].Id.toString();
                var displayDelete = "<td><button class='btn btn-danger' id=btnDelete name=" + stringId + ">Delete</button></td>";
                var displayEdit = "<td><button class='btn btn-warning' id=btnEdit name=" + stringId + ">Edit</button></td>";

                // prikaz samo ako je korisnik prijavljen
                if (token) {
                    row += displayData + displayDelete + displayEdit+ "</tr>";//prikazujemo display sa tokenom jer je tu puna tabela
                } else {
                    row += displayData + "</tr>";//displej sa osnovnim podacima koji nema token
                }
                // dodati red
                tbody.append(row);
            }
            table.append(tbody);

            div.append(table);

            $container.append(div);
        }
    }

    //kad se pritisne dugme registracija
    $("#regButt").click(function () {
        console.log(data);
        $("#info").empty().append("Enter your e-mail and password!");
        $("#regForm").css("display", "block");
        $("#regButt").css("display", "none");
        $("#logButt").css("display", "none");
       


    });
    //kad se pritisne dugme prijava
    $("#logButt").click(function () {
        console.log(data);
        $("#info").empty().append("Enter your e-mail and password!");
        $("#logForm").css("display", "block");
        $("#regButt").css("display", "none");
        $("#logButt").css("display", "none");
       


    });


    // registracija korisnika
    $("#regForm").submit(function (e) {
        e.preventDefault();

        var email = $("#regEmail").val();
        var pass1 = $("#regPass").val();
        var pass2 = $("#regPass1").val();

        // objekat koji se salje
        var sendData = {
            "Email": email,
            "Password": pass1,
            "ConfirmPassword": pass2
        };

        $.ajax({
            type: "POST",
            url: host + "/api/Account/Register",
            data: sendData

        }).done(function (data) {
            $("#regForm").css("display", "none");
            $("#logForm").css("display", "block");
            $("#regEmail").val("");
            $("#regLoz").val("");
            $("#regLoz2").val("");

        }).fail(function (data) {
            alert("Error!");
        });

    });
    //na klik dugmeta odustani u okviru forme registracije
    $("#back").click(function () {
        location.reload();
    });

    // prijava korisnika
    $("#logForm").submit(function (e) {
        e.preventDefault();

        var email = $("#logEmail").val();
        var pass = $("#logPass").val();

        // objekat koji se salje
        var sendData = {
            "grant_type": "password",
            "username": email,
            "password": pass
        };

        $.ajax({
            "type": "POST",
            "url": host + "/Token",
            "data": sendData

        }).done(function (data) {
            console.log(data);
            $("#info").empty().append("User: " + data.userName);
            token = data.access_token;

            $("#logForm").css("display", "none");
            $("#createForm").css("display", "block");
            $("#logOutButt").css("display", "block");
            $("#priEmail").val("");
            $("#priLoz").val("");
            loadProducts();

        }).fail(function (data) {
            alert("Error!");
        });
    });
    //na klik dugmeta odustani u okviru forme prijave
    $("#back1").click(function (e) {
        e.preventDefault();
        location.reload();
    });

    // odjava korisnika sa sistema
    $("#logOutButt").click(function () {
        token = null;
        headers = {};

        location.reload();
    });

    //brisanje 
    function deleteProducts() {

        var deleteID = this.name;

        // korisnik mora biti ulogovan
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        // saljemo zahtev 
        $.ajax({
            url: host + productsEndpoint + "/" + deleteID.toString(),
            type: "DELETE",
            headers: headers
        })
            .done(function (data, status) {
                loadProducts();
            })
            .fail(function (data, status) {
                alert("Error!");
            });
    }

   
    //dodavanje 
    $("#createForm").submit(function (e) {
        e.preventDefault();
        if (token) {
            headers.Authorization = "Bearer " + token;
        }

        var name = $("#nameCreate").val();
        var price = $("#priceCreate").val();
        var year = $("#yearCreate").val();
       


        var sendData = {
            
            "Name": name,
            "Price": price,
            "ProductionYear": year

        };

        $.ajax({
            "type": "POST",
            "url": host + productsEndpoint,
            "data": sendData,
            "headers": headers
        }).done(function () {
            
            $("#nameCreate").val("");
            $("#priceCreate").val("");
            $("#yearCreate").val("");
           


            loadProducts();
        }).fail(function (data, status) { alert("Error!"); });
    });


    //edit 
    function editProducts() {
        $("#editForm").css("display", "block");
        editId = this.name;

        if (token) {
            headers.Authorization = "Bearer " + token;
        }

        $.ajax({
            "type": "GET",
            "url": host + productsEndpoint + "/" + editId,
            "headers": headers
        }).done(function (data, status) {

            console.log(data);

            $("#nameEdit").val(data.Name);
            $("#priceEdit").val(data.Price);
            $("#yearEdit").val(data.ProductionYear);
           

        });
    }

    //izmena 
    $("#editForm").submit(function (e) {
        e.preventDefault();

        var name = $("#nameEdit").val();
        var price = $("#priceEdit").val();
        var year = $("#yearEdit").val();
        

        if (token) {
            headers.Authorization = "Bearer " + token;
        }


        var sendData = {
            "Id": editId,
            "Name": name,
            "Price": price,
            "ProductionYear": year
            

        };

        $.ajax({
            "type": "PUT",
            "url": host + productsEndpoint + "/" + editId,
            "data": sendData,
            "headers": headers
        }).done(function () {
            loadProducts();
            $("#editForm").css("display", "none");
        }).fail(function () { alert("Error!"); });



    });

    //odustajanje
    $("#back2").click(function (e) {
        e.preventDefault();

        $("#editForm").css("display", "none");
    });




});