import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnPendingmaterialissueComponent } from './ims-trn-pendingmaterialissue.component';

describe('ImsTrnPendingmaterialissueComponent', () => {
  let component: ImsTrnPendingmaterialissueComponent;
  let fixture: ComponentFixture<ImsTrnPendingmaterialissueComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnPendingmaterialissueComponent]
    });
    fixture = TestBed.createComponent(ImsTrnPendingmaterialissueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
