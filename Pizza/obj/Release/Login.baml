   M S B A M L   `   `   `  ����   Iceberg    testWPF.Login   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     �Name$  LoginWindow�� � Title$ Login��$	��150��$	��300��
����������	#FF282C30 � AllowsTransparency .  � WindowStyle$
 None=� � WindowStartupLocation$ CenterScreen=�.��� ���	#99282C30 � BindingGroup &   System.Windows.Data.BindingGroup  .+��� -    ��Name$ LoginTextBox��$��
10,35,10,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$��30�����	#99282C30$��Arial)�$��18.667(�
�������G� -    G�Name$ PasswordBox��$��
10,70,10,0q�$	��Top=�$��30�����	#99282C30	 G� Password$	  ��
��������� -   
 ��Name$
 LogButtonOk��$��Login��$
��Left=�$��
10,105,0,0q�$	��Top=�$��75�����	#FF282C30
������� �� 	IsDefault .  �� IsCancel . �� -   $
 LogButtonCanceled��$��Cancel��$
��Left=�$��215,105,0,0q�$	��Top=�$��75�����	#FF282C30
�������