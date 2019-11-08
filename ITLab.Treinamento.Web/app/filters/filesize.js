/**
 * Filesize Filter
 * @by          IT Lab
 * @author      marcelo.neias
 *
 * @Param length, default is 0
 * @return string
 *
 * Usage
 * {{ size | filesize }}, {{ size | filename:2 }}
 *
 * Output
 * 6 KB, 5.54 KB
 */
angular.module('itlabtreinamento')
  .filter('filesize', function () {
      var units = [
     'bytes',
     'KB',
     'MB',
     'GB',
     'TB',
     'PB'
      ];

      return function (bytes, precision) {
          if (isNaN(parseFloat(bytes)) || !isFinite(bytes)) {
              return '?';
          }

          var unit = 0;

          while (bytes >= 1024) {
              bytes /= 1024;
              unit++;
          }

          return bytes.toFixed(+precision) + ' ' + units[unit];
      };
  });

