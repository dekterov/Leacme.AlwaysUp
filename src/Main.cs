// Copyright (c) 2017 Leacme (http://leac.me). View LICENSE.md for more information.
using Godot;
using System;

public class Main : Spatial {

	public AudioStreamPlayer Audio { get; } = new AudioStreamPlayer();

	private void InitSound() {
		if (!Lib.Node.SoundEnabled) {
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), true);
		}
	}

	public override void _Notification(int what) {
		if (what is MainLoop.NotificationWmGoBackRequest) {
			GetTree().ChangeScene("res://scenes/Menu.tscn");
		}
	}

	public override void _Ready() {
		GetNode<WorldEnvironment>("sky").Environment.BackgroundColor = new Color(Lib.Node.BackgroundColorHtmlCode);
		InitSound();
		AddChild(Audio);

		GetNode<Spatial>("arrow").GetNode<MeshInstance>("Cylinder").MaterialOverride = new ShaderMaterial() {
			Shader = new Shader() {
				Code = @"
					shader_type spatial;
						void fragment(){
							ALBEDO = vec3(SCREEN_UV, 0f);
							EMISSION = vec3(SCREEN_UV, 0f);
						}"
			}
		};
	}

	public override void _Process(float delta) {
		if (Input.GetGravity().Length() > 0.1) {
			var tempAAT = GetNode<Spatial>("arrow").Transform;

			var rot = new Basis();
			rot.y = -Input.GetGravity().Normalized();
			rot.x = rot.y.Cross(new Vector3(1.0f, 0.0f, 0.0f)).Normalized();
			rot.z = rot.x.Cross(rot.y).Normalized();
			tempAAT.basis = rot;

			GetNode<Spatial>("arrow").Transform = tempAAT;
		}
	}

}
