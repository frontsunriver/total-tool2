@extends('layouts.backend.app')

@section('content')
<div class="card"  >
	<div class="card-body">
		<div class="row mb-30">
			<div class="col-lg-6">
				<h4>{{ __('Comment List') }}</h4>
			</div>
			<div class="col-lg-6 text-right">
				<a href="{{ route('admin.dsiqus.settings') }}" class="btn btn-primary"  >{{ __('Disqus Comment Settings') }}</a>
			</div>
		</div>
		<div class="cart-filter mb-20">
			<a href="{{ route('admin.comment.index') }}">All <span>({{ App\Comment::where('p_id',null)->count() }})</span></a>
			<a href="?qry=1">{{ __('Approved') }} <span>({{ App\Comment::where('status',1)->where('p_id',null)->count() }})</span></a>
			<a href="?qry=replyed">{{ __('Replyed') }} <span>({{ App\Comment::where('p_id',null)->where('status',4)->count() }})</span></a>
			<a href="?qry=adminreplyed">{{ __('Admin Replyed') }} <span>({{ App\Comment::where('p_id','!=',null)->where('status',4)->count() }})</span></a>
			
			<a href="?qry=2">{{ __('Pending') }} <span>({{ App\Comment::where('status',2)->where('p_id',null)->count() }})</span></a>
			<a href="?qry=3">{{ __('Unapproved') }} <span>({{ App\Comment::where('status',3)->where('p_id',null)->count() }})</span></a>
			<a href="?qry=trash" class="trash">{{ __('Trash') }} <span>({{ App\Comment::where('status',0)->where('p_id',null)->count() }})</span></a>
		</div>
		<div class="card-action-filter">
			<form method="post" id="basicform" action="{{ route('admin.comments.destroy') }}">
				@csrf
				<div class="row">
					<div class="col-lg-6">
						<div class="d-flex">
							<div class="single-filter">
								<div class="form-group">
									<select class="form-control" name="status">
										<option>{{ __('Select Action') }}</option>
										<option value="1">{{ __('Approved') }}</option>
										<option value="3">{{ __('Unapproved') }}</option>
										@if($status != 0)
										<option value="0">{{ __('Move to Trash') }}</option>
										@endif
										@if($status==0)
										<option value="delete">{{ __('Delete Permanently') }}</option>
										@endif
									</select>
								</div>
							</div>
							<div class="single-filter">
								<button type="submit" class="btn">{{ __('Apply') }}</button>
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
							<th class="am-title">{{ __('Author') }}</th>
							<th class="am-title">{{ __('Comment') }}</th>
							<th class="am-title">{{ __('Status') }}</th>
							<th class="am-title">{{ __('Submitted On') }}</th>
						</tr>
					</thead>
					<tbody>
						@foreach($comments as $comment)
						<tr>
							<th>
								<div class="custom-control custom-checkbox">
									<input type="checkbox" name="ids[]" class="custom-control-input" id="customCheck{{ $comment->id }}" value="{{ $comment->id }}">
									<label class="custom-control-label" for="customCheck{{ $comment->id }}"></label>
								</div>
							</th>
							<td>@if($comment->auth_id != null) {{ App\User::where('id',$comment->auth_id)->first()->name }} @endif</td>
							<td>
								{{ $comment->comment }}
								<div class="hover">
									@if($comment->status != 4)
									<a href="{{ route('admin.comment.show',$comment->id) }}">{{ __('Reply') }}</a>
									@endif
									
									<a href="{{ url('/post/'.$comment->term->slug) }}" target="_blank" class="last">{{ __('View') }}</a>
								</div>
							</td>
							<td>@if($comment->status==1)
								{{ __('Approved') }}
								@elseif($comment->status==2)
								{{ __('Pending') }}
								@elseif($comment->status==3)
								{{ __('Unapproved') }}
								@elseif($comment->status == 4)
								{{ __('Replyed') }}
								@else {{ __('Trash') }}
							@endif</td>
							<td>{{ __('Last Modified') }}
								<div class="date">
									{{ $comment->created_at->diffForHumans() }}
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
						<th class="am-title">{{ __('Author') }}</th>
						<th class="am-title">{{ __('Comment') }}</th>
						<th class="am-title">{{ __('Status') }}</th>
						<th class="am-title">{{ __('Submitted On') }}</th>
					</tr>
				</tfoot>
			</table>
			{{ $comments->links() }}
		</div>
	</div>
</div>


@endsection
@section('script')
<script src="{{ asset('admin/js/form.js') }}"></script>
<script type="text/javascript">
	"use strict";
	//response function
	function success(res){
		location.reload();
	}
</script>
@endsection