   M S B A M L   `   `   `  ����[  WLiveChartsCore.SkiaSharpView.WPF, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null}Xclr-namespace:LiveChartsCore.SkiaSharpView.WPF;assembly=LiveChartsCore.SkiaSharpView.WPF LiveChartsCore.SkiaSharpView.WPF  5       	MoneyMind   MoneyMind.StockPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      hlvcXclr-namespace:LiveChartsCore.SkiaSharpView.WPF;assembly=LiveChartsCore.SkiaSharpView.WPF     5        N� Title$  	StockPage��5      .��5      � ���#FFF8F0.+�5   
   �� 
����������#DAA520$��3q�5
       �� CornerRadius$ 20��5       �� Padding$ 20q�5      $	��400��5      $
��1000��5      $��Center=�5      $��Center=�5      ._�5      �� .+�5      �� $��Microsoft (MSFT)��6   $��26(�5      $
��Bold"�5      ���#DAA520$��0 0 0 20q�5      $��Center=�5      �� 5       �� Orientation$ 
Horizontal=�6   $��Center=�66   $��0 0 0 10q�6S   .+�5      �� $��Anzahl:��6    $��Center=�5       $��0 0 5 0q�5       $��14(�5       ���#2467955       �� 5       -   5         ��Name$ QuantityInput��5       $��80��5!      $��24��5"      $��0 0 10 0q�5#      ���#DAA520
���   �5%      �� 5&      -   5        $��Aktie kaufen��5&      ���#DAA520
�������$
��Bold"�5*      $	��6,2q�5+      $��0q�5,       � Cursor$
 Hand��5-      $	��100��5.      5/      �� 51      -   5         ��Name$ MarketStatusText��51      $4��.Market is closed – showing last known price.��52      $��14(�53      $��Italic$�54      
���  ��$��Center=�56      $��0 0 0 10q�57      $y�Visible=�58      �� 5:      -   5        $ LoadingText��5:      $��
Loading...��5;      $��16(�5<      ���#246795$��Center=�5>      $��0 0 0 10q�5?      5   /LiveChartsCore.SkiaSharpView.WPF.CartesianChart  5A      -   5          Name$ 
StockChart��5A   %   $	��300��5B   %      Series 5C   %   �� *Series+	   XAxes	 5D   %   �� *XAxes+
   YAxes
 5E   %   �� *YAxes+�� 5G      -   5        $ TotalValueText��5G      $��16(�5H      $
��Bold"�5I      ���#246795$��Center=�5K      $��
10 20 0 10q�5L      $+��%Gesamtwert gekaufter Aktien: CHF 0.00��5M      &   System.Windows.Controls.DataGrid  5O      -   5          Name$ StockDataGrid��5O         AutoGenerateColumns$ False��5P         HeadersVisibility$ Column=�5Q         
IsReadOnly$
 True��5R      $��0 10 0 0q�5S      $	��200��5T         CanUserResizeColumns$ False��5U         CanUserAddRows$ False��5V         GridLinesVisibility$
 None=�5W         RowBackground
 �����   AlternatingRowBackground �#FFF8F0���#DAA520$��1q�5[      
���   �   HorizontalGridLinesBrush �#DAA520   VerticalGridLinesBrush �#DAA520   Columns 5`      0  *System.Windows.Controls.DataGridTextColumn  5a      ,  &System.Windows.Controls.DataGridColumn   Header$ Datum��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6]   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   �� *PurchaseDate+6]     5b      $ Anzahl��6-   $ * 6Z    6=   �� *
Quantity+6Z     5c      $ 	Kaufpreis��6-   $ * 6b    6@   �� *PurchasePrice+6b     5d      $ Current Value��6-   $ * 6e    6D   �� *CurrentValue+6e   5f      5g      5h      5i      5j      