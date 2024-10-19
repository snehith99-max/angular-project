import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnStatustrackComponent } from './ims-trn-statustrack.component';

describe('ImsTrnStatustrackComponent', () => {
  let component: ImsTrnStatustrackComponent;
  let fixture: ComponentFixture<ImsTrnStatustrackComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnStatustrackComponent]
    });
    fixture = TestBed.createComponent(ImsTrnStatustrackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
