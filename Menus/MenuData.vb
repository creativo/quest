﻿Imports AxeSoftware.Quest.Controls.Menu

Friend Class MenuData

    Private m_modes As List(Of MenuMode)

    Public Sub New(ByVal ParamArray modes() As MenuMode)
        m_modes = New List(Of MenuMode)(modes)
    End Sub

    Public Function IsApplicable(ByVal mode As MenuMode) As Boolean
        Return m_modes.Contains(mode)
    End Function

End Class
