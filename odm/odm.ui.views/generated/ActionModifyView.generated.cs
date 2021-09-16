﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows.Input;
using odm.infra;
namespace odm.ui.activities {
	using global::onvif.services;
	
	public partial class ActionModifyView{
		
		#region Model definition
		
		public interface IModelAccessor{
			Action1 action{get;set;}
			SupportedActions supportedActions{get;set;}
			
		}
		public class Model: IModelAccessor, INotifyPropertyChanged{
			
			public Model(){
			}
			

			public static Model Create(
				Action1 action,
				SupportedActions supportedActions
			){
				var _this = new Model();
				
				_this.origin.action = action;
				_this.origin.supportedActions = supportedActions;
				_this.RevertChanges();
				
				return _this;
			}
		
				private SimpleChangeTrackable<Action1> m_action;
				private SimpleChangeTrackable<SupportedActions> m_supportedActions;

			private class OriginAccessor: IModelAccessor {
				private Model m_model;
				public OriginAccessor(Model model) {
					m_model = model;
				}
				Action1 IModelAccessor.action {
					get {return m_model.m_action.origin;}
					set {m_model.m_action.origin = value;}
				}
				SupportedActions IModelAccessor.supportedActions {
					get {return m_model.m_supportedActions.origin;}
					set {m_model.m_supportedActions.origin = value;}
				}
				
			}
			public event PropertyChangedEventHandler PropertyChanged;
			private void NotifyPropertyChanged(string propertyName){
				var prop_changed = this.PropertyChanged;
				if (prop_changed != null) {
					prop_changed(this, new PropertyChangedEventArgs(propertyName));
				}
			}
			
			public Action1 action  {
				get {return m_action.current;}
				set {
					if(m_action.current != value) {
						m_action.current = value;
						NotifyPropertyChanged("action");
					}
				}
			}
			
			public SupportedActions supportedActions  {
				get {return m_supportedActions.current;}
				set {
					if(m_supportedActions.current != value) {
						m_supportedActions.current = value;
						NotifyPropertyChanged("supportedActions");
					}
				}
			}
			
			public void AcceptChanges() {
				m_action.AcceptChanges();
				m_supportedActions.AcceptChanges();
				
			}

			public void RevertChanges() {
				this.current.action= this.origin.action;
				this.current.supportedActions= this.origin.supportedActions;
				
			}

			public bool isModified {
				get {
					if(m_action.isModified)return true;
					if(m_supportedActions.isModified)return true;
					
					return false;
				}
			}

			public IModelAccessor current {
				get {return this;}
				
			}

			public IModelAccessor origin {
				get {return new OriginAccessor(this);}
				
			}
		}
			
		#endregion
	
		#region Result definition
		public abstract class Result{
			private Result() { }
			
			public abstract T Handle<T>(
				
				Func<Model,T> apply,
				Func<Model,T> cancel,
				Func<T> close
			);
	
			public bool IsApply(){
				return AsApply() != null;
			}
			public virtual Apply AsApply(){ return null; }
			public class Apply : Result {
				public Apply(Model model){
					
					this.model = model;
					
				}
				public Model model{ get; set; }
				public override Apply AsApply(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> apply,
					Func<Model,T> cancel,
					Func<T> close
				){
					return apply(
						model
					);
				}
	
			}
			
			public bool IsCancel(){
				return AsCancel() != null;
			}
			public virtual Cancel AsCancel(){ return null; }
			public class Cancel : Result {
				public Cancel(Model model){
					
					this.model = model;
					
				}
				public Model model{ get; set; }
				public override Cancel AsCancel(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> apply,
					Func<Model,T> cancel,
					Func<T> close
				){
					return cancel(
						model
					);
				}
	
			}
			
			public bool IsClose(){
				return AsClose() != null;
			}
			public virtual Close AsClose(){ return null; }
			public class Close : Result {
				public Close(){
					
				}
				
				public override Close AsClose(){ return this; }
				
				public override T Handle<T>(
				
					Func<Model,T> apply,
					Func<Model,T> cancel,
					Func<T> close
				){
					return close(
						
					);
				}
	
			}
			
		}
		#endregion

		public ICommand ApplyCommand{ get; private set; }
		public ICommand CancelCommand{ get; private set; }
		public ICommand CloseCommand{ get; private set; }
		
		IActivityContext<Result> activityContext = null;
		SingleAssignmentDisposable activityCancellationSubscription = new SingleAssignmentDisposable();
		bool activityCompleted = false;
		//activity has been completed
		event Action OnCompleted = null;
		//activity has been failed
		event Action<Exception> OnError = null;
		//activity has been completed successfully
		event Action<Result> OnSuccess = null;
		//activity has been canceled
		event Action OnCancelled = null;
		
		public ActionModifyView(Model model, IActivityContext<Result> activityContext) {
			this.activityContext = activityContext;
			if(activityContext!=null){
				activityCancellationSubscription.Disposable = 
					activityContext.RegisterCancellationCallback(() => {
						EnsureAccess(() => {
							CompleteWith(() => {
								if(OnCancelled!=null){
									OnCancelled();
								}
							});
						});
					});
			}
			Init(model);
		}

		
		public void EnsureAccess(Action action){
			if(!CheckAccess()){
				Dispatcher.Invoke(action);
			}else{
				action();
			}
		}

		public void CompleteWith(Action cont){
			if(!activityCompleted){
				activityCompleted = true;
				cont();
				if(OnCompleted!=null){
					OnCompleted();
				}
				activityCancellationSubscription.Dispose();
			}
		}
		public void Success(Result result) {
			CompleteWith(() => {
				if(activityContext!=null){
					activityContext.Success(result);
				}
				if(OnSuccess!=null){
					OnSuccess(result);
				}
			});
		}
		public void Error(Exception error) {
			CompleteWith(() => {
				if(activityContext!=null){
					activityContext.Error(error);
				}
				if(OnError!=null){
					OnError(error);
				}
			});
		}
		public void Cancel() {
			CompleteWith(() => {
				if(activityContext!=null){
					activityContext.Cancel();
				}
				if(OnCancelled!=null){
					OnCancelled();
				}
			});
		}
		public void Success(Func<Result> resultFactory) {
			CompleteWith(() => {
				var result = resultFactory();
				if(activityContext!=null){
					activityContext.Success(result);
				}
				if(OnSuccess!=null){
					OnSuccess(result);
				}
			});
		}
		public void Error(Func<Exception> errorFactory) {
			CompleteWith(() => {
				var error = errorFactory();
				if(activityContext!=null){
					activityContext.Error(error);
				}
				if(OnError!=null){
					OnError(error);
				}
			});
		}
		public void Cancel(Action action) {
			CompleteWith(() => {
				action();
				if(activityContext!=null){
					activityContext.Cancel();
				}
				if(OnCancelled!=null){
					OnCancelled();
				}
			});
		}
		
	}
}
