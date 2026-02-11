using System;
using System.Media;
using System.Windows.Forms;
using Menedżer_notatek_i_rysunków.Models;
using Menedżer_notatek_i_rysunków.Services;

namespace Menedżer_notatek_i_rysunków
{
    public partial class AudioForm : Form
    {
        private readonly Note _note;
        private readonly IAudioService _audioService;
        private SoundPlayer? _player;

        public AudioForm(Note note, IAudioService audioService)
        {
            InitializeComponent();
            _note = note ?? throw new ArgumentNullException(nameof(note));
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
            audioName.Text = string.IsNullOrWhiteSpace(_note.Title) ? "Untitled" : _note.Title;
            playButton.Click += PlayButton_Click;
            stopButton.Click += StopButton_Click;
            deleteButton.Click += DeleteButton_Click;
            FormClosing += AudioForm_FormClosing;
        }

        private void PlayButton_Click(object? sender, EventArgs e)
        {
            try
            {
                string path = ResolveAudioPath();
                if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
                {
                    MessageBox.Show("Audio file not found.");
                    return;
                }

                _player?.Stop();
                _player = new SoundPlayer(path);
                _player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to play audio: " + ex.Message);
            }
        }

        private void StopButton_Click(object? sender, EventArgs e)
        {
            try
            {
                _player?.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to stop audio: " + ex.Message);
            }
        }

        private void DeleteButton_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Delete attached audio?", "Confirm", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes)
                return;

            try
            {
                _audioService.DeleteAudioForNote(_note);
                MessageBox.Show("Audio deleted.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete audio: " + ex.Message);
            }
        }

        private void AudioForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                _player?.Stop();
                _player = null;
            }
            catch { }
        }

        private string ResolveAudioPath()
        {
            if (_note.Audio != null)
            {
                var full = _note.Audio.GetFullPath();
                if (!string.IsNullOrWhiteSpace(full))
                    return full;
                return _note.Audio.FilePath ?? string.Empty;
            }

            return _audioService.GetAudioPathForNote(_note.Id) ?? string.Empty;
        }
    }
}
