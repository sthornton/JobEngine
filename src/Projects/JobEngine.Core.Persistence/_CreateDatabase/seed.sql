  INSERT INTO JobType
  (Id, Name)
  VALUES(1,'AssemblyJob')

    INSERT INTO JobType
  (Id, Name)
  VALUES(2,'Powershell')

  INSERT INTO LogLevel
  (Id, Name)
  VALUES (0, 'DEBUG')
	
  INSERT INTO LogLevel
  (Id, Name)
  VALUES (1, 'INFO')

  INSERT INTO LogLevel
  (Id, Name)
  VALUES (2, 'WARN')

  INSERT INTO LogLevel
  (Id, Name)
  VALUES (3, 'ERROR')

  INSERT INTO LogLevel
  (Id, Name)
  VALUES (4, 'FATAL')

  INSERT INTO PSResultType
  ( Name)
  VALUES ( 'Table')

    INSERT INTO PSResultType
  ( Name)
  VALUES ( 'String')

   INSERT INTO JobExecutionStatus
  ( Id, Name)
  VALUES (0, 'NOT_RECIEVED_BY_CLIENT')

      INSERT INTO JobExecutionStatus
  ( Id, Name)
  VALUES (1, 'DOWNLOADED_BY_CLIENT')

      INSERT INTO JobExecutionStatus
  ( Id, Name)
  VALUES (2, 'EXECUTING')

      INSERT INTO JobExecutionStatus
  ( Id, Name)
  VALUES (3, 'SUCCESS')

      INSERT INTO JobExecutionStatus
  ( Id, Name)
  VALUES (4, 'WARNING')

      INSERT INTO JobExecutionStatus
  ( Id, Name)
  VALUES (5, 'FAILED')

      INSERT INTO JobExecutionStatus
  ( Id, Name)
  VALUES (6, 'ERROR')

        INSERT INTO JobExecutionStatus
  ( Id, Name)
  VALUES (7, 'FATAL')


INSERT DataType (Id, NAME)
VALUES (1,'Int32')

INSERT DataType (Id, NAME)
VALUES (2,'Long')

INSERT DataType (Id, NAME)
VALUES (3,'String')