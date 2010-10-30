﻿Imports Microsoft.Win32

Friend Class RecentItems
    Private Const k_regPathPrefix As String = "Software\Quest"
    Private m_listName As String
    Private m_regPath As String

    Private m_recent As GameListItems

    Public Sub New(ByVal listName As String)
        m_listName = listName
        m_regPath = k_regPathPrefix + "\" + listName
        LoadRecentList()
    End Sub

    Public Sub AddToRecent(ByVal filename As String, ByVal name As String)
        If m_recent.Contains(filename) Then
            m_recent.Remove(filename)
        End If

        m_recent.Add(New GameListItemData(filename, name))
        SaveRecentList()
    End Sub

    Private Sub LoadRecentList()
        Dim key As RegistryKey
        key = Registry.CurrentUser.CreateSubKey(m_regPath)
        Dim values As List(Of String) = New List(Of String)(key.GetValueNames)

        m_recent = New GameListItems

        For Each value As String In values
            If value.StartsWith("Recent") Then
                Dim nameValue As String
                Dim name As String = Nothing
                nameValue = "Name" + value.Substring(6)
                If values.Contains(nameValue) Then
                    name = DirectCast(key.GetValue(nameValue), String)
                End If

                If String.IsNullOrEmpty(name) Then name = "(unknown)"

                m_recent.Add(New GameListItemData(DirectCast(key.GetValue(value), String), name))
            End If
        Next
    End Sub

    Private Sub SaveRecentList()
        Dim key As RegistryKey
        key = Registry.CurrentUser.CreateSubKey(k_regPathPrefix)
        key.DeleteSubKey(m_listName)
        key = Registry.CurrentUser.CreateSubKey(m_regPath)

        Dim count As Integer = 1

        For Each value As GameListItemData In m_recent
            key.SetValue("Recent" + count.ToString(), value.Filename)
            key.SetValue("Name" + count.ToString(), value.GameName)
            count += 1
        Next
    End Sub

    Public ReadOnly Property Items() As GameListItems
        Get
            Return m_recent
        End Get
    End Property

    Public Sub PopulateGameList(ByVal gameListCtl As GameList)
        Dim recentList As List(Of GameListItemData)
        recentList = New List(Of GameListItemData)(Items)
        recentList.Reverse()    ' newest first
        gameListCtl.CreateListElements(recentList)
    End Sub
End Class
