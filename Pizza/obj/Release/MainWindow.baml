   M S B A M L   `   `   `  ���� 	  Pizza    Pizza.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     �Name$  MyWindow�� � AllowsTransparency .  � WindowStyle$
 None=� � Icon$ Resources/Pizza.ico�� � WindowStartupLocation$ CenterScreen=� � Title$ Pizza Player��$	��600��$
��1000�����	#FF282C30$
��Auto=� J� Opacity$	 100Q�.��� -   ���	#99282C30 � ColumnDefinitions �� $��0* ��� �� $
��Auto �$	��774�� � RowDefinitions �� $	��32* ��� $	��21* ��� $
��537* ��� $��10 �$��10��.+��� -   $��ListMain��$��Hidden=�$��Hidden=�$��2��$��0q�$��Arial)�$��14(�
�����w����	#00282C30$	 �� IsSynchronizedWithCurrentItem$	 FalseY�$��1,1,0,0q�$��2��
 �� View
 �� -   
MyGridView��   MyGridViewCustomHeaderStyle! �� ColumnHeaderContainerStyle# �   .P��� $��Artist�� �� HeaderStringFormat$ String�� �� Width$	 720�� �� DisplayMemberBinding �� *ArtistAndTitle+�� $
��Time��$ String��$ 50�� �� *
Duration+�� -    ��Name$ 
PlayButton��$
��Left=�$��	74,20,0,0q�$	��Top=�$��19��$��19��
�������$��1��$��1��$��0q�$��2������  �� ImageSource$. (Resources/gnome_media_playback_start.ico�� j� Stretch$ UniformToFill=�����  �� 
TargetType" ��.R��� $��Border��#��z��#��z��#��z��._��� #��z��#��z��#��z�� �� Triggers P�  P� Property � �� P� Value . . ���  �� 
TargetName$ Border�� �� Property � �� �� Value
 ���� �� -   $ PauseButton��$
��Left=�$��89,0,0,0q�$	��Top=�$��19��$��19��
�������$��2��$��1��$��0q����� $. (Resources/gnome_media_playback_pause.ico��$ Uniform=����� " ��.R��� $��Border��#��z��#��z��#��z��._��� #��z��#��z��#��z�� P�  � �� . . ��� $ Border�� � ��
 ���� �� -   $ 
StopButton��$
��Left=�$��	104,0,0,0q�$��19��$��19��
�������$��2��$��1��$	��Top=�$��0q����� $- 'Resources/gnome_media_playback_stop.ico��$ Uniform=����� " ��.R��� $��Border��#��z��#��z��#��z��._��� #��z��#��z��#��z�� P�  � �� . . ��� $ Border�� � ��
 ���� �� -   $ NexttackButton��$��	140,0,0,0q�$��19��$��19��
�������$��2��$��1��$	��Top=�$
��Left=�$��0q����� $, &Resources/gnome_media_seek_forward.ico��$ Uniform=����� " ��.R��� $��Border��#��z��#��z��#��z��._��� #��z��#��z��#��z�� P�  � �� . . ��� $ Border�� � ��
 ���� �� -	   $ PreviousTrackButton��$
��Left=�$��	119,0,0,0q�$	��Top=�
�������$��2��$��1��$��19��$��19��$��0q����� $- 'Resources/gnome_media_seek_backward.ico��$ Uniform=����� " ��.R��� $��Border��#��z��#��z��#��z��._��� #��z��#��z��#��z�� P�  � �� . . ��� $ Border�� � ��
 ���� d� -
    d�Name$ ShuffleButton��$��0,14,184,503q�$��2��$��1��$��0q�$��Right=�$��24������ $0 *Resources/gnome_media_playlist_shuffle.ico��$ Uniform=����� " d�.R��� $��Border��#��z��#��z��#��z��._��� #��z��#��z��#��z�� P�  �@d�	IsChecked$
 TrueY�. ��� $ Border�� � �� �� $7 1Resources/gnome_media_playlist_shufflechecked.ico��$ Uniform=�d� -   $ RepeatButton��$��0,14,15,503q�$��2��$��1��$��0q�$��Right=�$��24������ $/ )Resources/gnome_media_playlist_repeat.ico��$ Uniform=����� " d�.R��� $��Border��#��z��#��z��#��z��._��� #��z��#��z��#��z�� P�  �@d�	IsChecked$
 TrueY�. ��� $ Border�� � �� �� $6 0Resources/gnome_media_playlist_repeatchecjed.ico��$ Uniform=��� -    ��Name$ ProgresSlider��  MySliderStyle#��[ $��	5,1,5,536q� � Minimum$ 0Q� � Maximum$	 100Q� �� AutoToolTipPlacement$ TopLeft=�$��1��$��1��$��2���� -   $ VolumeSlider��#��[ $��53,94,46,417q�$ TopLeft=�$ 0Q�$ 1Q� � LargeChange$	 0.1Q� � ToolTip$ Volume control��$��2��$��1��  � Value  �� ! �� ElementName$! MediaElement��" �� Path$" Volume�# �� Mode$# TwoWay=�r� -   $ r�Name$$ MediaElement��% r� LoadedBehavior$% Manual=�$
��Left=�$��43��$��	211,3,0,0q�$	��Top=�$��66��$y�Hidden=�$��0��$��2��$��2���� -   & ��Name$& CurrentTimeLabel��$��00:00��$��74,0,65,383q�$��Arial)�$��30(�
��� ���$��2��$��1��$��47��$��Bottom=��� -   $& TotalTimelabel��$��00:00��$��Right=�$��	0,9,270,0q�$	��Top=�$��Arial)�$��14(�
�������$��1��$��2��$��34��$��59��$��2��$y�Hidden=��� -   $& IcebergTextLabel��$��Pizza Player��$��0��$��arial)�$��22(�
��� ���$	��Top=�$��3��$��0��$��2��$��
46,0,120,0q�$��35���� -   $& UserIdTextLabel��$�� ��$��0,0,1,3q�$��arial)�$��32(�
�������$��2��$��Right=�$��Bottom=�$��2��$��47��$��10��A� -   ' A�Name$' 
UserAvatar��$��0��$��2��( J� RenderTransformOrigin$( 0.5,0.5&�$��2��$y�Visible=�$��77,20,73,444q��� -   $ 	MinButton��$
��Left=�$��	748,0,0,0q�$��15��$��15��
�������$��0��$��2��$	��Top=�$��0q����� $  Resources/MinButton-01.png��$ Uniform=����� " ��.R��� ) ��Name$) Border��#��z��#��z��#��z��._��� #��z��#��z��#��z�� P�  � �� . . ��� $ Border�� � ��
 ���� �� -   $ CloseButton��$
��Left=�$��	762,0,0,0q�$��15��$��15��$��0��$��2��$	��Top=�$��0q����� $" Resources/CloseButton-01.png��$ Uniform=����� " ��.R��� $) Border��#��z��#��z��#��z��._��� #��z��#��z��#��z�� P�  � �� . . ��� $ Border�� � ��
 ���� �� -   $ 
TestButton��$
��Left=�$��
10,512,0,0q�$��15��$��15��
�������$��2��$��1��$	��Top=�$��0q����� $  Resources/MinButton-01.png��$ Uniform=����� " ��.R��� $) Border��#��z��#��z��#��z��._��� #��z��#��z��#��z�� P�  � �� . . ��� $ Border�� � ��
 ���� 