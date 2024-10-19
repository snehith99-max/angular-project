import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnRequestedissueComponent } from './ims-trn-requestedissue.component';

describe('ImsTrnRequestedissueComponent', () => {
  let component: ImsTrnRequestedissueComponent;
  let fixture: ComponentFixture<ImsTrnRequestedissueComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnRequestedissueComponent]
    });
    fixture = TestBed.createComponent(ImsTrnRequestedissueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
