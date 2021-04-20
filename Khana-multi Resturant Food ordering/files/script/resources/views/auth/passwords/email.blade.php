<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, shrink-to-fit=no" name="viewport">
  <title>Reset &mdash; {{ env('APP_NAME') }}</title>

  <link rel="stylesheet" href="{{ asset('admin/assets/css/bootstrap.min.css') }}">
  <link rel="stylesheet" href="{{ asset('admin/assets/css/fontawesome.min.css') }}">

  <!-- Template CSS -->
  <link rel="stylesheet" href="{{ asset('admin/assets/css/style.css') }}">
  <link rel="stylesheet" href="{{ asset('admin/assets/css/components.css') }}">
</head>

<body>
  <div class="verify-email-address">
    <div class="container">
      <div class="row justify-content-center">
        <div class="col-md-8">
          <div class="card text-center">
           

            <div class="card-body">
             <div class="sign-in-section">
              <h1 class="mb-30">Reset Password</h1>
              @if (session('status'))
              <div class="alert alert-success" role="alert">
                {{ session('status') }}
              </div>
              @endif
              <form method="POST" action="{{ route('password.email') }}">
                @csrf
                <div class="form-field mb-10">
                  <label for="email">Email</label>
                  <input id="email" type="email" placeholder="Email" class="form-control @error('email') is-invalid @enderror" name="email" value="{{ old('email') }}" required autocomplete="email" autofocus/>
                  @error('email')
                  <span class="invalid-feedback" role="alert">
                    <strong>{{ $message }}</strong>
                  </span>
                  @enderror
                </div>
                <div class="form-field">
                  <input type="submit" class="btn btn-primary" value="Send Password Reset Link" />
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
</body>
</html>
































