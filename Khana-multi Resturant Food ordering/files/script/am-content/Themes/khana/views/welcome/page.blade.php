@extends('theme::layouts.app')
@section('content')
<div class="container">
<div class="row pt-50">
	<div class="col-sm-12">
		<div class="wrapper">
			<h3>{{ $info->title }}</h3>
			<p>{!! $info->content->content !!}</p>
		</div>
	</div>
</div>
</div>
@endsection