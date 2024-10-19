import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetingsPanelComponent } from './meetings-panel.component';

describe('MeetingsPanelComponent', () => {
  let component: MeetingsPanelComponent;
  let fixture: ComponentFixture<MeetingsPanelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MeetingsPanelComponent]
    });
    fixture = TestBed.createComponent(MeetingsPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
