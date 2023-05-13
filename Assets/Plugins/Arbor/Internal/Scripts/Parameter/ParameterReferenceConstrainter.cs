//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ParameterReferenceの参照するパラメータの型に依存して制約をかけるためのクラス
	/// </summary>
#else
	/// <summary>
	/// A class for constraining depending on the type of the parameter referenced by ParameterReference
	/// </summary>
#endif
	public sealed class ParameterReferenceConstrainter
	{
		private readonly System.Action<Parameter.Type, System.Type> _OnChangedType;
		private readonly System.Action<Parameter.Type> _OnDestroy;

		private ParameterReference _ParameterReference;
		private Parameter.Type _SlotParameterType;
		private System.Type _SlotReferenceType;

		private Parameter _Parameter;

#if ARBOR_DOC_JA
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="onChangedType">参照するパラメーターの型が変更された場合に呼ばれるコールバックを設定する。</param>
		/// <param name="onDestroy">参照するパラメーターが削除された場合に呼ばれるコールバックを設定する。</param>
#else
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="onChangedType">Set a callback that will be called when the type of the referenced parameter changes.</param>
		/// <param name="onDestroy">Set a callback that will be called when the referenced parameter is deleted.</param>
#endif
		public ParameterReferenceConstrainter(System.Action<Parameter.Type, System.Type> onChangedType, System.Action<Parameter.Type> onDestroy)
		{
			_OnChangedType = onChangedType;
			_OnDestroy = onDestroy;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約を実行する。
		/// </summary>
		/// <param name="parameterReference">制約に依存するパラメーターへの参照</param>
		/// <param name="slotParameterType">ParameterContainerの参照タイプがDataSlotだった場合に参照するタイプ</param>
		/// <param name="slotReferenceType">ParameterContainerの参照タイプがDataSlotだった場合に参照する値の型</param>
#else
		/// <summary>
		/// Execute the constraint.
		/// </summary>
		/// <param name="parameterReference">References to parameters that depend on constraints</param>
		/// <param name="slotParameterType">Type to refer to when the reference type of ParameterContainer is DataSlot</param>
		/// <param name="slotReferenceType">The type of the value to be referenced when the reference type of ParameterContainer is DataSlot.</param>
#endif
		public void Constraint(ParameterReference parameterReference, Parameter.Type slotParameterType, System.Type slotReferenceType)
		{
			if (_ParameterReference != parameterReference)
			{
				_ParameterReference = parameterReference;

				if (_Parameter != null)
				{
					_Parameter.onDestroy -= OnDestroyParameter;
					_Parameter.onAfterDeserialize -= OnAfterDeserializeParameter;
					_Parameter = null;
				}
			}

			_SlotParameterType = slotParameterType;
			_SlotReferenceType = slotReferenceType;

			InitializeConstraint(false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制約を破棄する。
		/// </summary>
#else
		/// <summary>
		/// Discard the constraint.
		/// </summary>
#endif
		public void Destroy()
		{
			if (_Parameter != null)
			{
				_Parameter.onDestroy -= OnDestroyParameter;
				_Parameter.onAfterDeserialize -= OnAfterDeserializeParameter;
				_Parameter = null;
			}
		}

		void InitializeConstraint(bool isDeserialized)
		{
			if (_ParameterReference == null)
			{
				return;
			}

			Parameter parameter = null;
			Parameter.Type parameterType = Parameter.Type.Int;
			System.Type referenceType = null;

			switch (_ParameterReference.type)
			{
				case ParameterReferenceType.Constant:
					{
						var containerBase = _ParameterReference.constantContainer;
						if (containerBase != null)
						{
							var defaultContainer = containerBase.defaultContainer;
							if (defaultContainer != null)
							{
								if (isDeserialized || defaultContainer.isDeserialized)
								{
									parameter = defaultContainer.GetParam(_ParameterReference.id);
									if (parameter != null)
									{
										parameterType = parameter.type;
										referenceType = parameter.referenceType;
									}
								}
								else
								{
									defaultContainer.onAfterDeserialize -= OnAfterDeserializeContainer;
									defaultContainer.onAfterDeserialize += OnAfterDeserializeContainer;
									return;
								}
							}
						}
					}
					break;
				case ParameterReferenceType.DataSlot:
					{
						parameterType = _SlotParameterType;
						referenceType = _SlotReferenceType;
					}
					break;
			}

			_OnChangedType?.Invoke(parameterType, referenceType);

			if (_Parameter != null)
			{
				_Parameter.onDestroy -= OnDestroyParameter;
				_Parameter.onAfterDeserialize -= OnAfterDeserializeParameter;
			}

			_Parameter = parameter;

			if (_Parameter != null)
			{
				_Parameter.onDestroy += OnDestroyParameter;
				_Parameter.onAfterDeserialize += OnAfterDeserializeParameter;
			}
		}

		void OnDestroyParameter()
		{
			if (_Parameter != null)
			{
				var parameterType = _Parameter.type;
				_OnDestroy?.Invoke(parameterType);
			}
			_Parameter = null;
		}

		void OnAfterDeserializeParameter()
		{
			InitializeConstraint(true);
		}

		void OnAfterDeserializeContainer()
		{
			InitializeConstraint(true);
		}
	}
}
