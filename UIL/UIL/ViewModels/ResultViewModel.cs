using BL;
using DAL;
using NetTopologySuite.Index.HPRtree;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using UIL.Commands;
using UIL.Logging;

namespace UIL.ViewModels
{
    public class ResultViewModel : BaseViewModel
    {
        public event EventHandler<tours?>? SelectedItemChanged;
        public event EventHandler<tourlogs?>? SelectedLogChanged;
        private static LoggerWrapper logger = ILoggerWrapperFactory.GetLogger();

        public ObservableCollection<tours> Items { get; } = new ObservableCollection<tours>();
        public ObservableCollection<tourlogs> TourLogItems { get; } = new ObservableCollection<tourlogs>();
        public event EventHandler<string?>? TextChanged;

        public ICommand CreateTourLog { get; }
        public ICommand EditTour { get; }
        public ICommand DeleteTour { get; }
        public ICommand EditTourLog { get; }
        public ICommand DeleteTourLog { get; }
        public ICommand CreateTour { get; }
        public ICommand SaveToFile { get; }
        public ICommand ReadFromFile { get; }
        public ICommand CompleteReport { get; }
        public ICommand TourReport { get; }
        private ImageSource mapImage;
        public ImageSource MapImage
        {
            get { return mapImage; }
            set
            {
                mapImage = value;
                OnPropertyChanged(nameof(MapImage));
            }
        }
        private string? _name;
        private string? _description;
        private string? _from;
        private string? _to;
        private string? _type;
        public string? _Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string? _Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        public string? _From
        {
            get => _from;
            set
            {
                _from = value;
                OnPropertyChanged();
            }
        }
        public string? _To
        {
            get => _to;
            set
            {
                _to = value;
                OnPropertyChanged();
            }
        }
        public string? _Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        private string? _time;
        private string? _distance;
        private string? _rating;
        private string? _comment;
        public string? time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }
        public string? distance
        {
            get => _distance;
            set
            {
                _distance = value;
                OnPropertyChanged();
            }
        }
        public string? rating
        {
            get => _rating;
            set
            {
                _rating = value;
                OnPropertyChanged();
            }
        }
        public string? comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }
        private string? filename;
        public string? FileName
        {
            get => filename;
            set
            {
                filename = value;
                OnPropertyChanged();
            }
        }
        private tours? selectedItem;
        public tours? SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged();
                OnSelectedItemChanged();
            }
        }
        public string picturename { get; set; }
        private tourlogs? selectedLog;
        public tourlogs? SelectedLog
        {
            get => selectedLog;
            set
            {
                selectedLog = value;
                OnPropertyChanged();
                OnSelectedTourLogChanged();
            }
        }
        public void SetItems(List<tours> tours)
        {
            Items.Clear();
            tours.ToList().ForEach(j => Items.Add(j));
        }
        public void SetTourLogs(List<tourlogs> logs)
        {
            TourLogItems.Clear();
            logs.ToList().ForEach(j => TourLogItems.Add(j));
        }
        public void SetPicture(string picture)
        {
            picturename = picture;
        }
        TourLogHandler tourLog = new TourLogHandler();
        TourHandler tour = new BL.TourHandler();


        public ResultViewModel()
        {
            
            
            EditTour = new CommandHandler((_) =>
            {
                if (SelectedItem != null)
                {
                    
                    string response = tour.EditTourForDB(SelectedItem.id, SelectedItem.tourname, SelectedItem.description, SelectedItem.startpoint, SelectedItem.endpoint, SelectedItem.type);
                    if(response == "ok")
                    {
                        logger.Info("Tour correctly edited");
                    }
                    else if(response == "noresult")
                    {
                        logger.Warning("No tour to edit found");
                    }
                    else if(response == "error")
                    {
                        logger.Fatal("Something went really wrong");
                    }
                }
                else
                {
                    logger.Fatal("Tried to edit tour without selecting tour");
                }


            });
            DeleteTour = new CommandHandler((_) =>
            {
                if (SelectedItem != null)
                {
                    tour.DeleteTourForDB(SelectedItem.id);
                }
                else
                {
                    logger.Fatal("Tried to delete tour without selecting tour");
                }

            });
            DeleteTourLog = new CommandHandler((_) =>
            {
                if (SelectedLog != null)
                {
                    tourLog.DeleteTourLogForDB(SelectedLog.id);
                }
                else
                {
                    logger.Fatal("Tried to delete tourlog without selecting tourlog");
                }
            });
            EditTourLog = new CommandHandler((_) =>
            {
                if (SelectedLog != null)
                {
                    tourLog.EditTourLogForDB(SelectedLog.id, SelectedLog.tourid, SelectedLog.time, SelectedLog.distance, SelectedLog.rating, SelectedLog.comment);
                }
                else
                {
                    logger.Fatal("Tried to edit tourlog without selecting tourlog");
                }
            });
            CreateTourLog = new CommandHandler((_) =>
            {
                
                    TextChanged?.Invoke(this, time);
                    TextChanged?.Invoke(this, distance);
                    TextChanged?.Invoke(this, rating);
                    TextChanged?.Invoke(this, comment);
                if (SelectedItem.id != null && time != null && distance != null && rating != null && comment != null)
                {
                    tourLog.CreateTourLogForDB(SelectedItem.id, time, distance, rating, comment);
                }
                else
                {
                    logger.Fatal("Tried to create tourlog without filling out the complete form");
                }
                
            });
            CreateTour = new CommandHandler((_) =>
            {
                TextChanged?.Invoke(this, _Name);
                TextChanged?.Invoke(this, _Description);
                TextChanged?.Invoke(this, _From);
                TextChanged?.Invoke(this, _To);
                TextChanged?.Invoke(this, _Type);
                if (_Name != null && _Description != null && _From != null && _To != null && _Type != null)
                {
                    tour.CreateTourForDB(_Name, _Description, _From, _To, _Type);
                }
                else
                {
                    logger.Fatal("Tried to create tour without filling out the complete form");
                }
            });
            SaveToFile = new CommandHandler((_) =>
            {
                if (SelectedItem != null)
                {
                    FileHandler handler = new FileHandler();
                    handler.SaveToFile(SelectedItem);
                }
                else
                {
                    logger.Warning("Tried to save a tour to file without selecting a tour");
                }
            });
            ReadFromFile = new CommandHandler((_) =>
            {
                TextChanged?.Invoke(this, FileName);
                if (FileName != null)
                {
                    
                    FileHandler handler = new FileHandler();
                    handler.ReadFromFile(FileName);
                }
                else
                {
                    logger.Warning("Tried to read a tour from file without writing a filename");
                }
                
            });
            CompleteReport = new CommandHandler((_) =>
            {
                FileHandler handler = new FileHandler();
                handler.CompleteReport();
            });
            TourReport = new CommandHandler((_) =>
            {
                if (SelectedItem != null) {
                    FileHandler handler = new FileHandler();
                    handler.TourReport(SelectedItem);
                    }
                else
                {
                    logger.Warning("Tried to create a single tourreport without selecting a tour");
                }
            });
            
    }
        private void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke(this, SelectedItem);
        }
        private void OnSelectedTourLogChanged()
        {
            SelectedLogChanged?.Invoke(this, SelectedLog);
        }
        
        
    }
    
}
