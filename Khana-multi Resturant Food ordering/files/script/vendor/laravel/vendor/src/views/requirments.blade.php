<!doctype html>
<html class="no-js" lang="">
<head>
  <meta charset="utf-8">
  <meta http-equiv="x-ua-compatible" content="ie=edge">
  <title>{{ __('Installer') }}</title>
  <meta name="description" content="">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="shortcut icon" type="image/x-icon" href="{{ asset('uploads/favicon.ico') }}">
  <!-- Place favicon.ico in the root directory -->

  <!-- CSS here -->
  <link rel="stylesheet" href="{{ asset('admin/css/bootstrap.min.css') }}">
  <link rel="stylesheet" href="{{ asset('admin/css/fontawesome-all.min.css') }}">
  <link rel="stylesheet" href="{{ asset('admin/css/font.css') }}">
  <link rel="stylesheet" href="{{ asset('admin/css/default.css') }}">
  <link rel="stylesheet" href="{{ asset('admin/css/style.css') }}">

</head>
<body class="install">

        <!--[if lte IE 9]>
            <p class="browserupgrade">You are using an <strong>outdated</strong> browser. Please <a href="https://browsehappy.com/">upgrade your browser</a> to improve your experience and security.</p>
          <![endif]-->

          <?php 
          $phpversion = phpversion();
          $mbstring = extension_loaded('mbstring');
          $bcmath = extension_loaded('bcmath');
          $ctype = extension_loaded('ctype');
          $json = extension_loaded('json');
          $openssl = extension_loaded('openssl');
          $pdo = extension_loaded('pdo');
          $tokenizer = extension_loaded('tokenizer');
          $xml = extension_loaded('xml');
          $fileinfo = extension_loaded('fileinfo');
          $fopen = ini_get('allow_url_fopen');

          $info = [
            'phpversion' => $phpversion,
            'mbstring' => $mbstring,
            'bcmath' => $bcmath,
            'ctype' => $ctype,
            'json' => $json,
            'openssl' => $openssl,
            'pdo' => $pdo,
            'tokenizer' => $tokenizer,
            'xml' => $xml,
            'fileinfo' => $fileinfo,
            'allow_url_fopen' => $fopen,
          ];
          ?>

          <!-- requirments-section-start -->
          <section class="mt-50 mb-50">
            <div class="requirments-section">
              <div class="content-requirments d-flex justify-content-center">
                <div class="requirments-main-content">
                  <div class="installer-header text-center">
                    <h2>{{ __('Requirments') }}</h2>
                    <p>{{ __('Please make sure the PHP extentions listed below are installed') }}</p>
                  </div>
                  <table class="table requirments">
                    <thead class="thead-light">
                      <tr>
                        <th scope="col">{{ __('Extentions') }}</th>
                        <th scope="col">{{ __('Status') }}</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr>
                        <td>{{ __('PHP >= 7.2.5') }}</td>
                        <td>
                          <?php 
                          if ($info['phpversion'] >= 7.2) { ?>
                            <i class="fas fa-check"></i>
                            <?php
                          }else{ ?>
                            <i class="fas fa-times"></i>
                            <?php
                          } 
                          ?>
                        </td>
                      </tr>
                      <tr>
                        <td>{{ __('BCMath PHP Extension') }}</td>
                        <td>
                          <?php 
                          if ($info['bcmath'] == 1) { ?>
                            <i class="fas fa-check"></i>
                            <?php
                          }else{ ?>
                            <i class="fas fa-times"></i>
                            <?php
                          } 
                          ?>
                        </td>
                      </tr>
                      <tr>
                        <td>{{ __('Ctype PHP Extension') }}</td>
                        <td>
                         <?php 
                         if ($info['ctype'] == 1) { ?>
                          <i class="fas fa-check"></i>
                          <?php
                        }else{ ?>
                          <i class="fas fa-times"></i>
                          <?php
                        } 
                        ?>
                      </td>
                    </tr>
                    <tr>
                      <td>{{ __('JSON PHP Extension') }}</td>
                      <td>
                       <?php 
                       if ($info['json'] == 1) { ?>
                        <i class="fas fa-check"></i>
                        <?php
                      }else{ ?>
                        <i class="fas fa-times"></i>
                        <?php
                      } 
                      ?>
                    </td>
                  </tr>
                  <tr>
                    <td>{{ __('Mbstring PHP Extension') }}</td>
                    <td>
                      <?php 
                      if ($info['mbstring'] == 1) { ?>
                        <i class="fas fa-check"></i>
                        <?php
                      }else{ ?>
                        <i class="fas fa-times"></i>
                        <?php
                      } 
                      ?>
                    </td>
                  </tr>
                  <tr>
                    <td>{{ __('OpenSSL PHP Extension') }}</td>
                    <td>
                      <?php 
                      if ($info['openssl'] == 1) { ?>
                        <i class="fas fa-check"></i>
                        <?php
                      }else{ ?>
                        <i class="fas fa-times"></i>
                        <?php
                      } 
                      ?>
                    </td>
                  </tr>
                  <tr>
                    <td>{{ __('PDO PHP Extension') }}</td>
                    <td>
                      <?php 
                      if ($info['pdo'] == 1) { ?>
                        <i class="fas fa-check"></i>
                        <?php
                      }else{ ?>
                        <i class="fas fa-times"></i>
                        <?php
                      } 
                      ?>
                    </td>
                  </tr>
                  <tr>
                    <td>{{ __('Tokenizer PHP Extension') }}</td>
                    <td>
                      <?php 
                      if ($info['tokenizer'] == 1) { ?>
                        <i class="fas fa-check"></i>
                        <?php
                      }else{ ?>
                        <i class="fas fa-times"></i>
                        <?php
                      } 
                      ?>
                    </td>
                  </tr>
                  <tr>
                    <td>{{ __('XML PHP Extension') }}</td>
                    <td>
                      <?php 
                      if ($info['xml'] == 1) { ?>
                        <i class="fas fa-check"></i>
                        <?php
                      }else{ ?>
                        <i class="fas fa-times"></i>
                        <?php
                      } 
                      ?>
                    </td>
                  </tr>
                  <tr>
                    <td>{{ __('Fileinfo PHP Extension') }}</td>
                    <td>
                      <?php 
                      if ($info['fileinfo'] == 1) { ?>
                        <i class="fas fa-check"></i>
                        <?php
                      }else{ ?>
                        <i class="fas fa-times"></i>
                        <?php
                      } 
                      ?>
                    </td>
                  </tr>
                  <tr>
                    <td>{{ __('Fopen PHP Extension') }}</td>
                    <td>
                      <?php 
                      if ($info['allow_url_fopen'] == 1) { ?>
                        <i class="fas fa-check"></i>
                        <?php
                      }else{ ?>
                        <i class="fas fa-times"></i>
                        <?php
                      } 
                      ?>
                    </td>
                  </tr>
                </tbody>
              </table>
              <?php 
              $page_url = (isset($_SERVER['HTTPS']) ? "https" : "http") . "://$_SERVER[HTTP_HOST]$_SERVER[REQUEST_URI]";
              if ($info['phpversion'] >= 7.2 && $info['mbstring'] == 1 && $info['bcmath'] == 1 && $info['ctype'] == 1 && $info['json'] == 1 && $info['openssl'] == 1 && $info['pdo'] == 1 && $info['tokenizer'] == 1 && $info['xml'] == 1 && $info['fileinfo'] == 1 && $info['allow_url_fopen'] == 1) { ?>
                <a href="{{ route('purchase') }}" class="btn btn-primary install-btn f-right">{{ __('Next') }}</a>
                <?php
              }else{ ?>
               <a href="#" class="btn btn-primary f-right disabled">{{ __('next') }}</a>
               <?php
             }
             ?>
           </div>
         </div>
       </div>
     </section>

     <!-- requirments-section-end -->

   </body>
   </html>
