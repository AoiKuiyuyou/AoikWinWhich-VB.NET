''
Imports System.IO

''
Module AoikWinWhich

    Function find_executable(ByVal prog As String) As List(Of String)
        '' 8f1kRCu
        Dim env_var_PATHEXT = Environment.GetEnvironmentVariable("PATHEXT")
        ''# can be Nothing

        '' 6qhHTHF
        '' split into a list of extensions
        Dim ext_s = If(env_var_PATHEXT = Nothing,
                New List(Of String)(),
                New List(Of String)(env_var_PATHEXT.Split(Path.PathSeparator))
        )

        '' 2pGJrMW
        '' strip
        ext_s = ext_s.Select(Function(x) x.Trim()).ToList()

        '' 2gqeHHl
        '' remove empty
        ext_s = ext_s.Where(Function(x) x <> "").ToList()

        '' 2zdGM8W
        '' convert to lowercase
        ext_s = ext_s.Select(Function(x) x.ToLower()).ToList()

        '' 2fT8aRB
        '' uniquify
        ext_s = ext_s.Distinct().ToList()

        '' 4ysaQVN
        Dim env_var_PATH = Environment.GetEnvironmentVariable("PATH")
        ''# can be Nothing

        Dim dir_path_s = If(env_var_PATH = Nothing,
                New List(Of String)(),
                New List(Of String)(env_var_PATH.Split(Path.PathSeparator))
        )

        '' 5rT49zI
        '' insert empty dir path to the beginning
        ''
        '' Empty dir handles the case that |prog| is a path, either relative or
        ''  absolute. See code 7rO7NIN.
        dir_path_s.Insert(0, "")

        '' 2klTv20
        '' uniquify
        dir_path_s = dir_path_s.Distinct().ToList()

        ''
        Dim prog_lc = prog.ToLower()

        Dim prog_has_ext = ext_s.Any(Function(ext) prog_lc.EndsWith(ext))

        '' 6bFwhbv
        Dim exe_path_s = New List(Of String)

        For Each dir_path In dir_path_s
            '' 7rO7NIN
            '' synthesize a path with the dir and prog
            Dim file_path = If(dir_path = "",
                    prog,
                    Path.Combine(dir_path, prog)
            )

            '' 6kZa5cq
            '' assume the path has extension, check if it is an executable
            If prog_has_ext And File.Exists(file_path) Then
                exe_path_s.Add(file_path)
            End If

            '' 2sJhhEV
            '' assume the path has no extension
            For Each ext In ext_s
                '' 6k9X6GP
                '' synthesize a new path with the path and the executable extension
                Dim path_plus_ext = file_path + ext

                '' 6kabzQg
                '' check if it is an executable
                If File.Exists(path_plus_ext) Then
                    exe_path_s.Add(path_plus_ext)
                End If
            Next
        Next

        '' 8swW6Av
        '' uniquify
        exe_path_s = exe_path_s.Distinct().ToList()

        ''
        Return exe_path_s
    End Function

    Sub Main()
        '' 9mlJlKg
        Dim args = Environment.GetCommandLineArgs()
        ''# first arg is this program's path.

        If args.Length <> 2 Then
            '' 7rOUXFo
            '' print program usage
            Console.WriteLine("Usage: aoikwinwhich PROG")
            Console.WriteLine("")
            Console.WriteLine("#/ PROG can be either name or path")
            Console.WriteLine("aoikwinwhich notepad.exe")
            Console.WriteLine("aoikwinwhich C:\Windows\notepad.exe")
            Console.WriteLine("")
            Console.WriteLine("#/ PROG can be either absolute or relative")
            Console.WriteLine("aoikwinwhich C:\Windows\notepad.exe")
            Console.WriteLine("aoikwinwhich Windows\notepad.exe")
            Console.WriteLine("")
            Console.WriteLine("#/ PROG can be either with or without extension")
            Console.WriteLine("aoikwinwhich notepad.exe")
            Console.WriteLine("aoikwinwhich notepad")
            Console.WriteLine("aoikwinwhich C:\Windows\notepad.exe")
            Console.WriteLine("aoikwinwhich C:\Windows\notepad")

            '' 3nqHnP7
            Return
        End If

        '' 9m5B08H
        '' get name or path of a program from cmd arg
        Dim prog = args(1)

        '' 8ulvPXM
        '' find executables
        Dim path_s = find_executable(prog)

        '' 5fWrcaF
        '' has found none, exit
        If path_s.Count = 0 Then
            '' 3uswpx0
            Return
        End If

        '' 9xPCWuS
        '' has found some, output
        Dim txt = String.Join(Environment.NewLine, path_s)

        Console.WriteLine(txt)

        '' 4s1yY1b
        Return
    End Sub

End Module
