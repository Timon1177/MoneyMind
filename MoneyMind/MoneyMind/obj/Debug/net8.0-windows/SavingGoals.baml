   M S B A M L   `   `   `  ����D  @MoneyMind, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:MoneyMind	MoneyMind  5          MoneyMind.SavingGoals   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )localclr-namespace:MoneyMind     5        N� Title$  SavingGoals��5	      .��5      � ���#FFFFFF � RowDefinitions 5      �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5      �� $��Saving Goals��6   $��0��5      $��32(�5      $
��Bold"�5      ���#DAA520$��20 20 20 10q�5      $��Center=�5      �� 5      $��1��6   $��20q�6   .+�5      �� -   5        $��+ Add Saving Goal��5      $	��160��5      $��40��6   ���#DAA520
�������$
��Bold"�5      $��0q�6!   $
��Left=�65   $��0,0,0,10q�6P    � Cursor$
 Hand��6b   �� 5      -   5         ��Name$ SavingGoalForm��5      $y�	Collapsed=�6+   $��0,0,0,10q�6B   .+�5    
   ��  �� Orientation$ 
Horizontal=�6   $��0,0,0,5q�6.   .+�5!      �� $��Name:��6   $	��120��6#   $��Center=�6/   �� 5"      -   5         ��Name$ SavingGoalNameInput��5"      $	��200��61   5#      �� 5$   
   $ 
Horizontal=�6   $��0,0,0,10q�6.   .+�5%      �� $��Amount (CHF):��6   $	��120��6+   $��Center=�67   �� 5&      -   5        $ SavingGoalAmountInput��5&      $	��200��63   5'      �� 5(   
   $ 
Horizontal=�6   $��0,0,0,10q�6.   .+�5)      �� $��Deadline (yyyy-mm-dd): ��6   $	��120��65   $��Center=�6A   �� 5*      -   5        $ SavingGoalDeadlineInput��5*      $	��200��65   5+      �� 5,   
   $ 
Horizontal=�6   $
��Left=�6.   .+�5-      �� -   5        $
��Save��5-      ���#DAA520
�������$	��100��6g   $��0,0,10,0q�6s   �� 5.      -   5        $��Cancel��5.      
�������$	��100��6^   5/      50   	   51      �� 54      $��2��6   $
��Auto=�6    .��55      �� -   5        $ 
goalsPanel��55      $��20q�6'   57   	   58      59      5;      