var makale = new Vue({
    el: "#makale",
    data: {
        makaleverisi: []
    },
    mounted: function () {
        this.getirMakale();
    },
    methods: {
        getirMakale: function () {
            var vm = this;
            $.ajax({ url: "/Makale/GetirMakale", method: "POST" })
                .done(function (data) {
                    vm.makaleverisi = data.data;
                });
        }
    }

})