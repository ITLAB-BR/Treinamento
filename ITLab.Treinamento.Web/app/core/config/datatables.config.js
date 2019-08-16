/**
 * by           IT Lab
 * author:      camilla.vianna
 * description: Configuração default do datatables
 * 
 * author:      eduardo.silva
 * description: Implementado método appendButton para permitir adicionar botão em determinados grids
 * date:        26/02/2019
 * 
 */

(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('DatatablesOptions', DatatablesOptions)

    DatatablesOptions.$inject = ['DTOptionsBuilder'];
    function DatatablesOptions(DTOptionsBuilder) {
        var defaultButtons = [
            { extend: 'copy', text: i18n.t('label.copy') },
            { extend: 'csv' },
            { extend: 'excel', title: 'ExampleFile' },
            { extend: 'pdf', title: 'ExampleFile' },
            {
                extend: 'print',
                text: 'Imprimir',
                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');

                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ];

        var newOptions = function () {
            return DTOptionsBuilder.newOptions()
                // como usar o withDOM: https://datatables.net/examples/basic_init/dom.html
                .withDOM('<"html5buttons"B><"pull-left"f>rt<"pull-left"i>pl')
                .withPaginationType('full_numbers')
                .withButtons(defaultButtons)
                .withLanguage({
                    buttons: {
                        copyTitle: i18n.t('datatables:copy_title'),
                        copySuccess: i18n.t('datatables:copy_success')
                    },
                    'sEmptyTable': i18n.t('datatables:empty_table'),
                    'sInfo': i18n.t('datatables:info'),
                    'sInfoEmpty': i18n.t('datatables:info_empty'),
                    'sInfoFiltered': i18n.t('datatables:info_filtered'),
                    'sInfoPostFix': '',
                    'sInfoThousands': i18n.t('datatables:info_thousands'),
                    'sLengthMenu': i18n.t('datatables:length_menu'),
                    'sLoadingRecords': i18n.t('datatables:loading'),
                    'sProcessing': i18n.t('datatables:processing'),
                    'sSearch': i18n.t('datatables:search'),
                    'sZeroRecords': i18n.t('datatables:zero'),
                    'oPaginate': {
                        'sFirst': i18n.t('datatables:pag_first'),
                        'sLast': i18n.t('datatables:pag_last'),
                        'sNext': i18n.t('datatables:pag_next'),
                        'sPrevious': i18n.t('datatables:pag_prev'),
                    },
                    'oAria': {
                        'sSortAscending': i18n.t('datatables:sort_asc'),
                        'sSortDescending': i18n.t('datatables:sort_desc'),
                    }
                });
        };

        var result = newOptions();

        result.appendButtons = function (newButtons) {
            return newOptions().withButtons(defaultButtons.concat(newButtons));
        };

        return result;
    };
})();
