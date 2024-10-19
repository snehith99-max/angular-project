import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RenewalManagerListComponent } from './renewal-manager-list.component';

describe('RenewalManagerListComponent', () => {
  let component: RenewalManagerListComponent;
  let fixture: ComponentFixture<RenewalManagerListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RenewalManagerListComponent]
    });
    fixture = TestBed.createComponent(RenewalManagerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
