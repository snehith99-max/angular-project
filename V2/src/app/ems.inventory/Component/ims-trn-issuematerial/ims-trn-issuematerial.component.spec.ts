import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImsTrnIssuematerialComponent } from './ims-trn-issuematerial.component';

describe('ImsTrnIssuematerialComponent', () => {
  let component: ImsTrnIssuematerialComponent;
  let fixture: ComponentFixture<ImsTrnIssuematerialComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImsTrnIssuematerialComponent]
    });
    fixture = TestBed.createComponent(ImsTrnIssuematerialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
