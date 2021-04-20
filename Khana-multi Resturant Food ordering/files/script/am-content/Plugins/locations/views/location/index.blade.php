@extends('layouts.backend.app')

@section('content')
<div class="card"  >
	<div class="card-body">
		<div class="row mb-30">
			<div class="col-lg-6">
				<h4>{{ __('Location List') }}</h4>
			</div>
			<div class="col-lg-6">
				<div class="add-new-btn">
					<a href="{{ route('admin.location.create') }}" class="btn f-right btn-primary">{{ __('Add New') }}</a>
				</div>
			</div>
		</div>

		<div class="cart-filter mb-20">
			<a href="{{ route('admin.location.index') }}">{{ __('All') }} <span>({{ App\Terms::where('type',2)->where('status','!=',0)->count() }})</span></a>
			<a href="?st=1">{{ __('Published') }} <span>({{ App\Terms::where('type',2)->where('status',1)->count() }})</span></a>
			<a href="?st=2">{{ __('Drafts') }} <span>({{ App\Terms::where('type',2)->where('status',2)->count() }})</span></a>
			<a href="?st=trash" class="trash">{{ __('Trash') }} <span>({{ App\Terms::where('type',2)->where('status',0)->count() }})</span></a>
		</div>
		<div class="card-action-filter">
			<form method="post" id="basicform" action="{{ route('admin.locations.destroy') }}">
				@csrf
				<div class="row">
					<div class="col-lg-6">
						<div class="d-flex">
							<div class="single-filter">
								<div class="form-group">
									<select class="form-control" name="status">
										<option value="publish">{{ __('Publish') }}</option>
										<option value="trash">{{ __('Move to Trash') }}</option>
										<option value="delete">{{ __('Delete Permanently') }}</option>

									</select>
								</div>
							</div>
							<div class="single-filter">
								<button type="submit" class="btn btn-primary mt-1 ml-2">{{ __('Apply') }}</button>
							</div>
						</div>
					</div>
					<div class="col-lg-6">
						<div class="single-filter f-right">
							<div class="form-group">
								<input type="text" id="data_search" class="form-control" placeholder="Enter Value">
							</div>
						</div>
					</div>
				</div>
			</div>
			<div class="table-responsive custom-table">
				<table class="table">
					<thead>
						<tr>
							<th class="am-select">
								<div class="custom-control custom-checkbox">
									<input type="checkbox" class="custom-control-input checkAll" id="customCheck12">
									<label class="custom-control-label checkAll" for="customCheck12"></label>
								</div>
							</th>
							<th class="am-title"><i class="far fa-image"></i></th>
							<th class="am-title">{{ __('Title') }}</th>
							<th class="am-title">{{ __('Total Users') }}</th>
							<th class="am-title">{{ __('Status') }}</th>						
							<th class="am-date">{{ __('Date') }}</th>
						</tr>
					</thead>
					<tbody>
						@foreach($posts as $post)
						<tr>
							
							<th>
								@if ($post->userslocation_count <= 0)
								<div class="custom-control custom-checkbox">
									<input type="checkbox" name="ids[]" class="custom-control-input" id="customCheck{{ $post->id }}" value="{{ $post->id }}">
									<label class="custom-control-label" for="customCheck{{ $post->id }}"></label>
								</div>
								@endif
							</th>
							
							
							<td>
								<img src="{{ asset($post->preview->content ?? '') }}" height="50">
							</td>
							<td>
								{{ $post->title }}
								<div class="hover">
									<a href="{{ route('admin.location.edit',$post->id) }}">{{ __('Edit') }}</a>

									
								</div>
							</td>
							<td>{{ $post->userslocation_count }}</td>
							<td>@if($post->status==1)  {{ __('Published') }} @elseif($post->status==2)  Draft @else {{ __('Trash') }} @endif</td>
							
							
							<td>{{ __('Last Modified') }}
								<div class="date">
									{{ $post->updated_at->diffForHumans() }}
								</div>
							</td>
						</tr>
						@endforeach
					</tbody>
				</form>
				<tfoot>
					<tr>
						<th class="am-select">
							<div class="custom-control custom-checkbox">
								<input type="checkbox" class="custom-control-input checkAll" id="customCheck12">
								<label class="custom-control-label checkAll" for="customCheck12"></label>
							</div>
						</th>
						<th class="am-title"><i class="far fa-image"></i></th>
						<th class="am-title">{{ __('Title') }}</th>
						
						<th class="am-title">{{ __('Total Users') }}</th>
						<th class="am-title">{{ __('Status') }}</th>
						
						
						<th class="am-date">{{ __('Date') }}</th>
					</tr>
				</tfoot>
			</table>
			{{ $posts->links() }}
		</div>
	</div>
</div>
@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script type="text/javascript">
	"use strict";	
	//response will assign this function
	function success(res){
		location.reload();
	}
</script>
@endsection