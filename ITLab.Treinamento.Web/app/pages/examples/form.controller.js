(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('FormController', FormController);

    FormController.$inject = ['DaterangepickerOptions', '$timeout'];
    function FormController(DaterangepickerOptions, $timeout) {
        var vm = this;

        vm.basic = {
            text: 'Exemple',
            password: 'Exemple'
        };

        vm.checkbox = {
            check: {
                'default': true,
                primary: true,
                success: true,
                info: true,
                warning: true,
                danger: true
            },
            radio: 'default',
            inline: {
                'default': false,
                primary: true,
                success: false,
                info: true,
                warning: false,
                danger: false
            },
        };

        vm.switch = true;

        vm.selectOptions = [
            { Id: 1, Description: 'Opção exemplo 1' },
            { Id: 2, Description: 'Opção exemplo 2' },
            { Id: 3, Description: 'Opção exemplo 3' },
            { Id: 4, Description: 'Opção exemplo 4' },
            { Id: 5, Description: 'Opção exemplo 5' }
        ];
        vm.select = {
            simple: 2,
            multiple: [1, 2, 5],
            picklist: [1, 2, 5]
        };

        /*
          DaterangepickerOptions contém as configurações padrão do sistema para o datepicker 
          opções pré-definidas disponíveis para ranges:
             'today'
             'yesterday'
             'last_7_days'
             'this_week'
             'last_week'
             'last_30_days'
             'this_month'
             'last_month'
             'this_year'
             'last_year'
        */
        vm.dateOptionsSimple = DaterangepickerOptions.singleConfig;
        vm.dateOptionsRange = DaterangepickerOptions.rangeConfig;
        vm.dateOptionsPre = angular.copy(DaterangepickerOptions.rangeConfig);
        vm.dateOptionsPre.ranges = DaterangepickerOptions.ranges(['last_7_days', 'last_week', 'last_30_days', 'last_month']);
        vm.date = {
            simple: moment(),
            range: { startDate: moment(), endDate: moment().add(1, 'd') },
            rangePre: { startDate: null, endDate: null }
        };

        vm.color = '';

        vm.loadingButton = {
            loading: false,
            onClick: function () {
                vm.loadingButton.loading = true;
                $timeout(function () {
                    vm.loadingButton.loading = false;
                }, 1500);
            }
        };

        vm.numeral = {
            integer: 1234,
            decimal: 1234.5
        };

        vm.mask = {
            tel: 1212345678,
            rg: 121231239
        };

        vm.seeBasic = seeBasic;
        vm.seeCheckbox = seeCheckbox;
        vm.seeSwitch = seeSwitch;
        vm.seeSelect = seeSelect;
        vm.seeDate = seeDate;
        vm.seeColor = seeColor;
        vm.seeNumbers = seeNumbers;
        vm.seeMask = seeMask;


        function seeBasic() {
            console.log(vm.basic);
            var msg = 'Texto: ' + (vm.basic.text || '') + '<br>'
                    + 'Placeholder: ' + (vm.basic.placeholder || '') + '<br>'
                    + 'Senha: ' + (vm.basic.password || '');
            toastr.success(msg, 'Básico');
        };
        function seeCheckbox() {
            console.log(vm.checkbox);
            var msg = 'Checkbox: ' + '<br>'
                        + '&emsp;default: ' + vm.checkbox.check.default + '<br>'
                        + '&emsp;primary: ' + vm.checkbox.check.primary + '<br>'
                        + '&emsp;success: ' + vm.checkbox.check.success + '<br>'
                        + '&emsp;info: ' + vm.checkbox.check.info + '<br>'
                        + '&emsp;warning: ' + vm.checkbox.check.warning + '<br>'
                        + '&emsp;danger: ' + vm.checkbox.check.danger + '<br><br>'
                        + 'Radio: ' + vm.checkbox.radio
            toastr.success(msg, 'Checkbox');
        };
        function seeSwitch() {
            console.log(vm.switch);
            var msg = 'Switch: ' + vm.switch
            toastr.success(msg, 'Switch');
        };
        function seeSelect() {
            console.log(vm.select);
            var msg = 'Simples: ' + vm.select.simple + '<br>'
                    + 'Multiplo: ' + vm.select.multiple + '<br>'
                    + 'Picklist: ' + vm.select.picklist;
            toastr.success(msg, 'Select');
        };
        function seeDate() {
            console.log(vm.date);
            /* datepicker retorna um moment
             * para converter para formato Date, usar .toDate()
             * para formatar, use .format() ou .format('L')
             * vide: http://momentjs.com/
             */
            var msg = 'Simples (toDate): ' + vm.date.simple.toDate() + '<br><br>'
                    + 'Período (format): ' + vm.date.range.startDate.format('L') + ' ~ ' + vm.date.range.endDate.format('L');
            toastr.success(msg, 'Date');
        };
        function seeColor() {
            console.log(vm.color);
            var msg = 'Color: ' + vm.color
            toastr.success(msg, 'Color');
        };
        function seeNumbers() {
            console.log(vm.numeral);
            var msg = 'Inteiro: ' + vm.numeral.integer + '<br>'
                    + 'Decimal: ' + vm.numeral.decimal + '<br>'
                    + 'Formulário: ' + vm.numeral.input;
            toastr.success(msg, 'Números');
        };
        function seeMask() {
            console.log(vm.mask);
            var msg = 'Telefone ' + vm.mask.tel + '<br>'
                    + 'RG: ' + vm.mask.rg;
            toastr.success(msg, 'Máscara');
        };
    };
})();