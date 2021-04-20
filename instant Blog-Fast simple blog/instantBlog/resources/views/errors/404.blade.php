@extends('layouts.master')

@section('bodyclass')
   <body class="bg-instant">
@endsection

@section('content')
<div class="container mt-5">
  <!-- Example row of columns -->
    <div class="content">
        <div class="alert alert-warning" role="alert">
      		<strong>Nothing was found!</strong> Back to <a href="{{ url('/') }}">Home</a>.
    	</div>
    </div>
  </div> <!-- /container -->
@endsection